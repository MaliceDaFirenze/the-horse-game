﻿using System.Collections;
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
	private float maxIdleDuration = 12f;
	private WaitForSeconds waitASecond = new WaitForSeconds(1f);

	private Consumable currentTargetConsumable;
	private float reachDistToConsumable = 2;

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
			StartCoroutine (ProduceManure ());
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

				Debug.Log ("horse looking at  " + allConsumables [i].name + " dist: " + dist + ", isvisible: " + isVisible + " has remaining value: " + allConsumables[i].remainingNeedValue);
				if (dist < minDist && isVisible && allConsumables[i].remainingNeedValue > 0 && allConsumables[i].enabled) {
					minDist = dist;
					result = allConsumables [i];
				}
			}
		}

		return result;
	}

	private IEnumerator ProduceManure(){
		
	}
}
