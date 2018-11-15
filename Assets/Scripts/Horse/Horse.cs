using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public enum horseNeed {
	FOOD,
	WATER,
	HAPPINESS,
	HYGIENE
}

public class Horse : MonoBehaviour {

	//refereneces
	public Horse_Behavior horseBehavior;
	public Horse_Interactable horseInteractable;
	public Horse_Stats horseStats;
	public Horse_RidingBehavior horseRidingBehavior;

	public Animator horseAnimator;

	void Awake (){
		InitReferences ();
	}

	private void InitReferences(){
		horseBehavior = GetComponent<Horse_Behavior> ();
		if (horseBehavior == null) {
			Debug.Log ("No horseBehavior on " + name);
		} else {
			horseBehavior.horse = this;
		}

		horseInteractable = GetComponent<Horse_Interactable> ();
		if (horseInteractable == null) {
			Debug.Log ("No horseInteractable on " + name);
		} else {
			horseInteractable.horse = this;
		}

		horseStats = GetComponent<Horse_Stats> ();
		if (horseStats == null) {
			Debug.Log ("No horseStats on " + name);
		} else {
			horseStats.horse = this;
		}

		horseRidingBehavior = GetComponent<Horse_RidingBehavior> ();
		if (horseRidingBehavior == null) {
			Debug.Log ("No horseRidingBehavior on " + name);
		} else {
			horseRidingBehavior.horse = this;
		}

		horseAnimator = GetComponentInChildren<Animator> ();
		if (horseAnimator == null) {
			Debug.Log ("No horseAnimator on " + name);
		}

	}
}
