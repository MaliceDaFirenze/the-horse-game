using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum horseState{
	IDLE,
	CONSUMING,
	WALKINGTOTARGET
}

public class Horse_Behavior : MonoBehaviour {

	private Horse_Stats stats;
	public horseState activeHorseState;

	// Use this for initialization
	void Start () {
		stats = GetComponent<Horse_Stats> ();
	}
	
	// Update is called once per frame
	void Update () {
		//do coroutine instead of update-based state machines? an idle cycle takes a few seconds, after which the horse "decides" to do sth or start a new idle cycle?
		//but: stopping coroutines is a bitch, no?
		//or maybe it's not so bad if I just save a current behaviour coroutine? and then if sth happens (like: player whistles), I cancel the current behaviour and reevaluate my next actions
	}

	private void Idle(){
		if (stats.Food < Horse_Stats.NeedsMaximum) {

		}
	}

	private void WalkingToTarget(){
	
	}

	private void Consuming(){
	
	}

}
