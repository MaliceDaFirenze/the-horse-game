using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Horse_Behavior : MonoBehaviour {

	//---References---/
	public Horse horse;
	private NavMeshAgent navMeshAgent;

    private Collider colliderToAvoid = null;
    public bool isAvoidingCollider = false;
    public bool shouldAvoidInPositiveDirection = true;
    private float timeOfLastCollisionDetection = 0.0f;

	public horseState currentHorseState;
	private Coroutine currentBehaviour;
	public horseGait currentHorseGait;

	private Consumable currentTargetConsumable;
	private float reachDistToConsumable = 2;

	//---Durations & Waits---//
	private float minIdleDuration = 3f;
	private float maxIdleDuration = 12f;
	private WaitForSeconds waitASecond = new WaitForSeconds(1f);
	private WaitForSeconds waitShort = new WaitForSeconds(0.1f);
	private WaitForSeconds horseGaitChangeDelay = new WaitForSeconds(0.5f); //not bc I want delay but bc I don't want this to check every single frame

	private float minManureDuration = 20f;
	private float maxManureDuration = 40f;

	public LayerMask obstacleRaycastLayers;
	private bool jumpInProgress;

	void Start () {
		//Initialize
		navMeshAgent = GetComponent<NavMeshAgent> ();

		ChangeState (horseState.IDLE);
	}

	private void ChangeState(horseState newState){
		if (currentBehaviour != null) {
			StopCoroutine (currentBehaviour);
		}

		horse.horseAnimator.SetBool ("Eat", false);
		horse.horseAnimator.SetInteger ("GaitIndex", 0);

		currentHorseState = newState;
		switch (newState) {
		case horseState.IDLE:
			currentBehaviour = StartCoroutine (Idle ());
			break;
		case horseState.WALKINGTOTARGET:
			currentBehaviour = StartCoroutine (WalkToTarget ());
			break;
		case horseState.CONSUMING:
			currentBehaviour = StartCoroutine (Consume ());
			break;
		case horseState.PRODUCINGMANURE:
			currentBehaviour = StartCoroutine (ProduceManure());
			break;
		case horseState.ONLEAD:
			currentBehaviour = StartCoroutine (BeLead ());
			break;
		case horseState.RIDING:
			currentBehaviour = StartCoroutine (BeRidden ());
			break;
		case horseState.TIEDTOPOST:
			currentBehaviour = StartCoroutine (BeTiedToPost ());
			break;
		}
	}

	private IEnumerator Idle(){
		float idleDuration = Random.Range (minIdleDuration, maxIdleDuration);
		yield return new WaitForSeconds (idleDuration);
		Consumable availableFood = FindConsumableInRange (horseNeed.FOOD);
		Consumable availableWater = FindConsumableInRange (horseNeed.WATER);

		if (horse.horseStats.Food < Horse_Stats.NeedsMaximum * 0.9f && availableFood != null) {
			currentTargetConsumable = availableFood;
			ChangeState (horseState.WALKINGTOTARGET);
		} else if (horse.horseStats.Water < Horse_Stats.NeedsMaximum * 0.9f && availableWater != null) {
			currentTargetConsumable = availableWater;
			ChangeState (horseState.WALKINGTOTARGET);
		} else {
			ChangeState (horseState.IDLE);
		}
	}

	private IEnumerator WalkToTarget(){
		horse.horseAnimator.SetInteger ("GaitIndex", 1);
		navMeshAgent.SetDestination (currentTargetConsumable.transform.position);
		while (Vector3.Distance (currentTargetConsumable.transform.position, transform.position) > reachDistToConsumable && currentTargetConsumable != null && currentTargetConsumable.enabled && currentTargetConsumable.remainingNeedValue > 0) {
			yield return waitASecond;
		}

		//navMeshAgent.Stop ();
		navMeshAgent.isStopped = true;
		navMeshAgent.ResetPath ();
		horse.horseAnimator.SetInteger ("GaitIndex", 0);

		yield return new WaitForEndOfFrame ();

		ChangeState (horseState.CONSUMING);
	}

	private IEnumerator Consume(){

		bool hasEaten = false;

		while (currentTargetConsumable != null && Vector3.Distance (currentTargetConsumable.transform.position, transform.position) <= reachDistToConsumable && currentTargetConsumable.remainingNeedValue > 0 && horse.horseStats.GetNeedValue(currentTargetConsumable.needSatisfiedByThis) < Horse_Stats.NeedsMaximum){
			horse.horseAnimator.SetBool ("Eat", true);
			horse.horseStats.SatisfyNeed(currentTargetConsumable.needSatisfiedByThis, currentTargetConsumable.PartialConsume ());
			hasEaten = true;
			yield return waitASecond;
		}

		if (hasEaten) {
			StartCoroutine (WaitToProduceManure ());
		}

		horse.horseAnimator.SetBool ("Eat", false);
		ChangeState (horseState.IDLE);
	}

	private IEnumerator ProduceManure(){
		horse.horseAnimator.SetBool ("Poop", false);
		GameObject manurePile = Instantiate (PrefabManager.instance.manurePile, horse.horseStats.poopSpawnPoint.position, horse.horseStats.poopSpawnPoint.rotation) as GameObject;

		yield return StartCoroutine( manurePile.GetComponent<ManurePile>().GetProduced() );

		horse.horseAnimator.SetBool ("Poop", false);
		ChangeState (horseState.IDLE);
	}

	private IEnumerator BeLead(){ 
		horse.horseAnimator.SetBool ("Still", true);
		while (true) {
			horse.horseAnimator.SetInteger ("GaitIndex", (int)currentHorseGait);

			//with "changegaitbyriding setting the index, this coroutine isn't needed anymore for riding
			//need to set "Still" to true somewhere though? 
			//still need the coroutine for actual leading
			yield return horseGaitChangeDelay;
		}
	}

	private IEnumerator BeRidden(){
		//horse stays in this state until change from outside
		Player ridingPlayer = transform.root.GetComponent<Player>();

		while (true) {
			//only cast ahead if horse is moving
			if (ridingPlayer.PreviousMovementVector.magnitude > 0){// && !jumpInProgress) {
			
				//raycast ahead for obstacles
				//Vector3 castTarget = transform.position + transform.forward * 4f * mounted.actualMovementSpeedMultiplier;
                float yOffset = 1;
                Vector3 castOrigin = transform.position + transform.up*yOffset;
                Vector3 castDirection = transform.forward;
				RaycastHit hit;

				//works for slow canter. 
				//why doesn't it work for fast canter? 
                //Vector3 castDirection = castTarget - horse.horseStats.headBone.position;
				Debug.DrawRay (castOrigin, castDirection*100, Color.yellow, 0.5f);
				if (Physics.Raycast (castOrigin, castDirection, out hit, 100, obstacleRaycastLayers)) {
					Debug.DrawRay (hit.point, hit.normal, Color.cyan, 0.5f);

                    float distanceToObstacle = hit.distance;
					float angle = Vector3.Angle (hit.normal, castDirection);
					//the hit normal draws straight away from the obstacle. the angle is calculated in three dimensions, but I actually only care about x/z, not x/y or z/y. the hit point is below the ground

					float animationSpeed = horse.horseRidingBehavior.gaitAniSpeed;
                    float timeToJump = 2f/animationSpeed;
					float jumpDistance = horse.horseRidingBehavior.currentTotalMovementSpeed * timeToJump; // 2 is the total time of the animation before the height of the jump

                    float jumpMargin = 1;
                    float avoidTime = 1f;
					float timeToCollision = distanceToObstacle/horse.horseRidingBehavior.currentTotalMovementSpeed;
                    if (jumpDistance - jumpMargin < distanceToObstacle && distanceToObstacle < jumpDistance) {
					    if (180-angle < 20) {
						    StartCoroutine (Jump (hit.collider));
					    }
                    }                    
                    
                    if (hit.collider != colliderToAvoid) {
                        //Debug.Log("1:" + (Time.time - timeOfLastCollisionDetection));
                        if(Time.time - timeOfLastCollisionDetection > 0.4f) { 
                            isAvoidingCollider = false;
                            colliderToAvoid = null;
                        }
                    }
                    if (timeToCollision < avoidTime && !jumpInProgress) {
                        Debug.Log ("obstacle ahead and not jumping: " + hit.collider.name);
                        isAvoidingCollider = true;
                        colliderToAvoid = hit.collider;
                        shouldAvoidInPositiveDirection = (Vector3.SignedAngle (hit.normal, castDirection, transform.up) > 0.0f);
                        timeOfLastCollisionDetection = Time.time;
						//mounted.Stop ();
                    }

				} else { 
                        //Debug.Log("2:" + (Time.time - timeOfLastCollisionDetection));
                    if(Time.time - timeOfLastCollisionDetection > 0.4f) { 
                        isAvoidingCollider = false;
                        colliderToAvoid = null;
                    }
                }

			} else {
				//Debug.Log ("horse was standing still or jump in progress, not casting");
			}
				
			yield return waitShort;
		
		}
	}

	private void Update(){
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			Time.timeScale = 0.1f;
		} else if (Input.GetKeyDown(KeyCode.Alpha2)){
			Time.timeScale = 0.5f;
		} else if (Input.GetKeyDown(KeyCode.Alpha3)){
			Time.timeScale = 1.0f;
		}
	}


	private IEnumerator Jump(Collider colliderToJump){
        // If collider to specific obstace is already disabled, exit coroutine
        if(colliderToJump.enabled == false) { 
            yield break;
        }

		Debug.Log ("start jump coroutine");
		jumpInProgress = true;
        colliderToJump.enabled = false;
		horse.horseRidingBehavior.ignorePlayerInput = true;
        //if (jumpInProgress) { 
        //    anim.Play("Jump", -1, 0);
        //}
		horse.horseAnimator.SetBool ("Jump", true);

		yield return new WaitForSeconds (1f);
		horse.horseAnimator.SetBool ("Jump", false);
        yield return new WaitForSeconds (2f);

		currentHorseGait = horseGait.CANTER;
		horse.horseRidingBehavior.ignorePlayerInput = false;
        colliderToJump.enabled = true;
		jumpInProgress = false;
		Debug.Log ("end jump coroutine");
	}

	private IEnumerator BeTiedToPost(){
		horse.horseAnimator.SetBool ("Still", true);
		while (true) {
			yield return waitASecond;
		}
	}

	public void PutHorseOnLead(bool onLead){
		if (onLead) {
			ChangeState (horseState.ONLEAD);
			navMeshAgent.enabled = false;
		} else {
			navMeshAgent.enabled = true;
			horse.horseAnimator.SetBool ("Still", false);
			currentHorseGait = horseGait.STAND;
			ChangeState (horseState.IDLE);
		}
	}

	public void RidingHorse(bool mountUp){
		if (mountUp) {
			ChangeState (horseState.RIDING);
			navMeshAgent.enabled = false;
		} else {
			navMeshAgent.enabled = true;
			horse.horseAnimator.SetBool ("Still", false);
			currentHorseGait = horseGait.STAND;
			ChangeState (horseState.IDLE);
		}
	}

	public void TieHorseToPost(bool onPost){
		if (onPost) {
			navMeshAgent.enabled = false;
			ChangeState (horseState.TIEDTOPOST);
		} else {
			navMeshAgent.enabled = true;
			horse.horseAnimator.SetBool ("Still", false);
			ChangeState (horseState.IDLE);
		}
	}

	public void ChangeGaitByRiding(float newGaitWeight, float newGaitAniSpeed){
		horse.horseAnimator.SetFloat ("GaitSpeedWeight", newGaitWeight);
		horse.horseAnimator.SetFloat ("GaitAniSpeed", newGaitAniSpeed);

		horse.horseAnimator.SetInteger ("GaitIndex", (int)currentHorseGait);
	}

	private Consumable FindConsumableInRange(horseNeed need){
		Consumable result = null;
		Consumable[] allConsumables = FindObjectsOfType<Consumable> ();

		float maxDist = 60f;
		float minDist = maxDist;
		RaycastHit hit;
		for (int i = 0; i < allConsumables.Length; ++i) {
			if (allConsumables [i].needSatisfiedByThis == need) {
				float dist = Vector3.Distance (horse.horseStats.headBone.position, allConsumables [i].transform.position);
				bool isVisible = false;  
				Debug.DrawRay (horse.horseStats.headBone.position, allConsumables [i].transform.position - horse.horseStats.headBone.position, Color.red, 2f);
				if (Physics.Raycast (horse.horseStats.headBone.position, allConsumables [i].transform.position - horse.horseStats.headBone.position, maxDist, out hit)) {
					//Debug.Log ("horse looking for consumable, raycast hit " + hit.collider.name + " with tag: " + hit.collider.tag);
					if (hit.collider.tag.Equals("Consumable")){
						isVisible = true;
					}
				}

				//Debug.Log ("horse looking at  " + allConsumables [i].name + " dist: " + dist + ", isvisible: " + isVisible + " has remaining value: " + allConsumables[i].remainingNeedValue);
				if (dist < minDist && isVisible && allConsumables[i].remainingNeedValue > 0 && allConsumables[i].enabled) {
					minDist = dist;
					result = allConsumables [i];
				}
			}
		}

		return result;
	}

	public IEnumerator WaitToProduceManure(){
		float duration = Random.Range (minManureDuration, maxManureDuration);

		yield return new WaitForSeconds (duration);
		ChangeState (horseState.PRODUCINGMANURE);
	}
}
