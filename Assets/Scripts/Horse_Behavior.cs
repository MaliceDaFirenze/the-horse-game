using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum horseState{
	IDLE,
	CONSUMING,
	WALKINGTOTARGET
}

public class Horse_Behavior : MonoBehaviour {

	//---References---/
	private Horse_Stats stats;
	private Animator anim;
	private NavMeshAgent navMeshAgent;

	public horseState currentHorseState;
	private Coroutine currentBehaviour;

	private float minIdleDuration = 3f;
	private float maxIdleDuration = 5f;//12f;
	private WaitForSeconds waitASecond = new WaitForSeconds(1f);

	private Consumable currentTargetConsumable;

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
		}
	}

	private IEnumerator Idle(){
		float idleDuration = Random.Range (minIdleDuration, maxIdleDuration);
		Debug.Log ("new idle duration: " + idleDuration);
		yield return new WaitForSeconds (idleDuration);

		if (stats.Food < Horse_Stats.NeedsMaximum) {
			currentTargetConsumable = FindConsumableInRange (horseNeed.FOOD);
			ChangeState (horseState.WALKINGTOTARGET);
		} else if  (stats.Water < Horse_Stats.NeedsMaximum * 0.9f){
			currentTargetConsumable = FindConsumableInRange (horseNeed.WATER);
			ChangeState (horseState.WALKINGTOTARGET);
		}
	}

	private IEnumerator WalkToTarget(){
		Debug.Log ("start walking to: " + currentTargetConsumable);
		anim.SetBool ("Walk", true);
		navMeshAgent.SetDestination (currentTargetConsumable.transform.position);
		while (Vector3.Distance (currentTargetConsumable.transform.position, transform.position) > 2) {
			yield return waitASecond;
		}

		anim.SetBool ("Walk", false);
		ChangeState (horseState.CONSUMING);
	}

	private IEnumerator Consume(){
		if (Vector3.Distance (currentTargetConsumable.transform.position, transform.position) > 2){
			anim.SetBool ("Eat", true);
			yield return waitASecond;

		}
		anim.SetBool ("Eat", false);
		ChangeState (horseState.IDLE);
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
					Debug.Log ("horse looking for consumable, raycast hit " + hit.collider.name + " with tag: " + hit.collider.tag);
					if (hit.collider.tag.Equals("Consumable")){
						isVisible = true;
					}
				}

				Debug.Log ("horse looking at  " + allConsumables [i].name + " dist: " + dist + ", isvisible: " + isVisible);
				if (dist < minDist && isVisible) {
					minDist = dist;
					result = allConsumables [i];
				}
			}
		}
		return result;
	}
}
