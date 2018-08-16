﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse_Mounted : MonoBehaviour {

	//what does this need to do?

	//establish links to behaviour, maybe stats, interactable, player...
	//input can be handled in Player script, but needs to be forwarded to this, where
	//the speed up/down and gait up/down logic is handled

	private float gaitWeight;
	private float gaitAniSpeed;

	public float actualMovementSpeedMultiplier; //which is used by the player for the actual movement

	private float changeSpeedValueBy;
	private float changeSpeedValueByMin = 0.05f;
	private float changeSpeedValueByMax = 0.2f;

	private float minAniSpeed = 0.6f;
	private float maxAniSpeed = 1.5f;

	private void Start(){
		gaitWeight = 0.5f;
		gaitAniSpeed = 1f;
	}

	public void ReceivePlayerInput(Player player, dir input, Vector3 playerMovementVector){

		Debug.Log ("player input: " + input);

		//add speed meter within gait to UI

		changeSpeedValueBy = Random.Range (changeSpeedValueByMin, changeSpeedValueByMax);

		switch (input) {
		case dir.UP: 
			gaitAniSpeed += changeSpeedValueBy;
			if (gaitAniSpeed > maxAniSpeed) {
				gaitAniSpeed = maxAniSpeed;
			}
			break;
		case dir.DOWN:
			gaitAniSpeed -= changeSpeedValueBy;
			if (gaitAniSpeed < minAniSpeed) {
				gaitAniSpeed = minAniSpeed;
			}
			break;
		case dir.LEFT:
			break;
		case dir.RIGHT:
			break;
		}

		//gait weight: normalize to get val between 0 and 1

		//randomize: if you haven't given any input after a while, the horse might slow down or speed up on its own?
		//depending on its energy, motivation, shyness, surroundings? 

		horseBehaviour.ChangeGaitByRiding (gaitWeight, gaitAniSpeed);
		actualMovementSpeedMultiplier = gaitAniSpeed; //like this, I could also just have the player access gaitAniSpeed, or write a public getter for it? 

		Debug.Log ("new gait speed: " + gaitAniSpeed);

		//calc a value for both of these at once maybe, find a 'formula' to always have two values that fit together?
	}

	//references
	private Horse_Behavior _horseBehaviour;
	public Horse_Behavior horseBehaviour {
		get { 
			if (_horseBehaviour == null) {
				_horseBehaviour = GetComponent<Horse_Behavior> ();
			}
			return _horseBehaviour;
		}
	}
}
