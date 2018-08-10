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
	TIEDTOPOST
}

public enum horseGait{
	STAND,
	WALK,
	TROT,
	CANTER,
	GALLOP
}

public class Horse_Behavior : MonoBehaviour {

	//---References---/
	private Horse_Stats stats;
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

	private float minManureDuration = 20f;
	private float maxManureDuration = 40f;

	void Start () {
		//Initialize
		stats = GetComponent<Horse_Stats> ();
		navMeshAgent = GetComponent<NavMeshAgent> ();
		anim = GetComponentInChildren<Animator> ();

		ChangeState (horseState.IDLE);
	}

	private void ChangeState(horseState newState){
		if (currentBehaviour != null) {
			StopCoroutine (currentBehaviour);
		}

		anim.SetBool ("Eat", false);
		anim.SetBool ("Walk", false);

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
		anim.SetBool ("Walk", true);
		navMeshAgent.SetDestination (currentTargetConsumable.transform.position);
		while (Vector3.Distance (currentTargetConsumable.transform.position, transform.position) > reachDistToConsumable && currentTargetConsumable != null && currentTargetConsumable.enabled && currentTargetConsumable.remainingNeedValue > 0) {
			yield return waitASecond;
		}

		//navMeshAgent.Stop ();
		navMeshAgent.isStopped = true;
		navMeshAgent.ResetPath ();
		anim.SetBool ("Walk", false);
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
			switch (currentHorseGait) {
			case horseGait.STAND:
				anim.SetBool ("Still", true);
				anim.SetBool ("Trot", false);
				anim.SetBool ("Walk", false);
				break;
			case horseGait.WALK: 
				anim.SetBool ("Walk", true);
				anim.SetBool ("Trot", false);
				anim.SetBool ("Still", false);
				break;
			case horseGait.TROT: 
				anim.SetBool ("Trot", true);
				anim.SetBool ("Still", false);
				anim.SetBool ("Walk", false);
				break;
			}
			yield return waitASecond;
		}
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
		} else {
			anim.SetBool ("Still", false);
			currentHorseGait = horseGait.STAND;
			ChangeState (horseState.IDLE);
		}
	}
	public void TieHorseToPost(bool onPost){
		if (onPost) {
			ChangeState (horseState.TIEDTOPOST);
		} else {
			anim.SetBool ("Still", false);
			ChangeState (horseState.IDLE);
		}
	}

	public void ChangeGaitByRiding(){
		//change currenthorsegait
		//update bools
		//change state? or state is just "under saddle" "being ridden"?
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
