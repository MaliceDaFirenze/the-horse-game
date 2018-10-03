using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum horseState{
	IDLE,
	CONSUMING,
	WALKINGTOTARGET,
	PRODUCINGMANURE,
	ONLEAD,
	RIDING,
	TIEDTOPOST
}

public enum horseGait{
	STAND,
	WALK,
	TROT,
	CANTER
}

public class Horse_Behavior : MonoBehaviour {

	//---References---/
	private Horse_Stats stats;
	private Horse_Mounted mounted;
	private Animator anim;
	private NavMeshAgent navMeshAgent;

	public horseState currentHorseState;
	private Coroutine currentBehaviour;
	public horseGait currentHorseGait;

	private Consumable currentTargetConsumable;
	private float reachDistToConsumable = 2;

	//---Durations & Waits---//
	private float minIdleDuration = 3f;
	private float maxIdleDuration = 12f;
	private WaitForSeconds waitASecond = new WaitForSeconds(1f);
	private WaitForSeconds waitShort = new WaitForSeconds(0.2f);
	private WaitForSeconds horseGaitChangeDelay = new WaitForSeconds(0.5f); //not bc I want delay but bc I don't want this to check every single frame

	private float minManureDuration = 20f;
	private float maxManureDuration = 40f;

	public LayerMask obstacleRaycastLayers;
	private bool jumpInProgress;

	void Start () {
		//Initialize
		stats = GetComponent<Horse_Stats> ();
		navMeshAgent = GetComponent<NavMeshAgent> ();
		anim = GetComponentInChildren<Animator> ();
		mounted	= GetComponent<Horse_Mounted>();

		ChangeState (horseState.IDLE);
	}

	private void ChangeState(horseState newState){
		if (currentBehaviour != null) {
			StopCoroutine (currentBehaviour);
		}

		anim.SetBool ("Eat", false);
		anim.SetInteger ("GaitIndex", 0);

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

		if (stats.Food < Horse_Stats.NeedsMaximum * 0.9f && availableFood != null) {
			currentTargetConsumable = availableFood;
			ChangeState (horseState.WALKINGTOTARGET);
		} else if (stats.Water < Horse_Stats.NeedsMaximum * 0.9f && availableWater != null) {
			currentTargetConsumable = availableWater;
			ChangeState (horseState.WALKINGTOTARGET);
		} else {
			ChangeState (horseState.IDLE);
		}
	}

	private IEnumerator WalkToTarget(){
		anim.SetInteger ("GaitIndex", 1);
		navMeshAgent.SetDestination (currentTargetConsumable.transform.position);
		while (Vector3.Distance (currentTargetConsumable.transform.position, transform.position) > reachDistToConsumable && currentTargetConsumable != null && currentTargetConsumable.enabled && currentTargetConsumable.remainingNeedValue > 0) {
			yield return waitASecond;
		}

		//navMeshAgent.Stop ();
		navMeshAgent.isStopped = true;
		navMeshAgent.ResetPath ();
		anim.SetInteger ("GaitIndex", 0);

		yield return new WaitForEndOfFrame ();

		ChangeState (horseState.CONSUMING);
	}

	private IEnumerator Consume(){

		bool hasEaten = false;

		while (currentTargetConsumable != null && Vector3.Distance (currentTargetConsumable.transform.position, transform.position) <= reachDistToConsumable && currentTargetConsumable.remainingNeedValue > 0 && stats.GetNeedValue(currentTargetConsumable.needSatisfiedByThis) < Horse_Stats.NeedsMaximum){
			anim.SetBool ("Eat", true);
			stats.SatisfyNeed(currentTargetConsumable.needSatisfiedByThis, currentTargetConsumable.PartialConsume ());
			hasEaten = true;
			yield return waitASecond;
		}

		if (hasEaten) {
			StartCoroutine (WaitToProduceManure ());
		}

		anim.SetBool ("Eat", false);
		ChangeState (horseState.IDLE);
	}

	private IEnumerator ProduceManure(){
		anim.SetBool ("Poop", false);
		GameObject manurePile = Instantiate (PrefabManager.instance.manurePile, stats.poopSpawnPoint.position, stats.poopSpawnPoint.rotation) as GameObject;

		yield return StartCoroutine( manurePile.GetComponent<ManurePile>().GetProduced() );

		anim.SetBool ("Poop", false);
		ChangeState (horseState.IDLE);
	}

	private IEnumerator BeLead(){ 
		anim.SetBool ("Still", true);
		while (true) {
			anim.SetInteger ("GaitIndex", (int)currentHorseGait);

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
			if (ridingPlayer.PreviousMovementVector.magnitude > 0 && !jumpInProgress) {
			
				//raycast ahead for obstacles
				Vector3 castTarget = transform.position + transform.forward * 4f * mounted.actualMovementSpeedMultiplier;
				RaycastHit hit;

				//works for slow canter. 
				//why doesn't it work for fast canter? 

				Debug.DrawRay (stats.headBone.position, castTarget - stats.headBone.position, Color.yellow, 0.5f);
				if (Physics.Raycast (stats.headBone.position, castTarget - stats.headBone.position, out hit, 100, obstacleRaycastLayers)) {
					Debug.DrawRay (hit.point, hit.normal, Color.cyan, 0.5f);
					float angle = Vector3.Angle (hit.normal, castTarget - stats.headBone.position);

					//the hit normal draws straight away from the obstacle. the angle is calculated in three dimensions, but I actually only care about x/z, not x/y or z/y. the hit point is below the ground

					Debug.Log ("angle " + angle + ". 180-angle: " + (180-angle));

					if (180-angle < 20) {
						Debug.Log ("can jump");
						StartCoroutine (Jump ());
					} else {
						Debug.Log ("obstacle ahead: " + hit.collider.name);
						mounted.Stop ();
					}
				} 

			} else {
				//Debug.Log ("horse was standing still or jump in progress, not casting");
			}
				
			yield return waitShort;
		
		}
	}

	private IEnumerator Jump(){
		Debug.Log ("start jump coroutine");
		jumpInProgress = true;
		Physics.IgnoreLayerCollision (0, 8, true);
		mounted.ignorePlayerInput = true;
		anim.SetBool ("Jump", true);

		yield return new WaitForSeconds (3f);

		anim.SetBool ("Jump", false);
		currentHorseGait = horseGait.CANTER;
		mounted.ignorePlayerInput = false;
		Physics.IgnoreLayerCollision (0, 8, false);
		jumpInProgress = false;
		Debug.Log ("end jump coroutine");
	}

	private IEnumerator BeTiedToPost(){
		anim.SetBool ("Still", true);
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
			anim.SetBool ("Still", false);
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
			anim.SetBool ("Still", false);
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
			anim.SetBool ("Still", false);
			ChangeState (horseState.IDLE);
		}
	}

	public void ChangeGaitByRiding(float newGaitWeight, float newGaitAniSpeed){
		anim.SetFloat ("GaitSpeedWeight", newGaitWeight);
		anim.SetFloat ("GaitAniSpeed", newGaitAniSpeed);

		anim.SetInteger ("GaitIndex", (int)currentHorseGait);
	}

	private Consumable FindConsumableInRange(horseNeed need){
		Consumable result = null;
		Consumable[] allConsumables = FindObjectsOfType<Consumable> ();

		float minDist = 200f;
		RaycastHit hit;
		for (int i = 0; i < allConsumables.Length; ++i) {
			if (allConsumables [i].needSatisfiedByThis == need) {
				float dist = Vector3.Distance (stats.headBone.position, allConsumables [i].transform.position);
				bool isVisible = false;  
				Debug.DrawRay (stats.headBone.position, allConsumables [i].transform.position - stats.headBone.position, Color.red, 2f);
				if (Physics.Raycast (stats.headBone.position, allConsumables [i].transform.position - stats.headBone.position, out hit)) {
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
