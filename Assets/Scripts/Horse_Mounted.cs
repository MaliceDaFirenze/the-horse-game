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

	public float actualMovementSpeedMultiplier = 1f; //which is used by the player for the actual movement
	private float speedAdjustmentModifier = 0.5f;

	private float changeSpeedValueBy;
	private float changeSpeedValueByMin = 0.05f;
	private float changeSpeedValueByMax = 0.2f;

	private float minAniSpeed = 0.85f;
	private float maxAniSpeed = 1.7f;

	private HorseRidingUI ui;

	private float pressedLeftLastTime = 0f;
	private float pressedRightLastTime = 0f;
	private float gaitChangeTapInterval = 0.6f;

	private void Start(){

		ui = FindObjectOfType<HorseRidingUI> ();
	}

	private void Update(){
		//only if mounted is active / rider is on horse
		//gradually move speed towards 0.5 and ani speed towards 1 (depending on horse mood / energy / character though)
	
	}

	public void MountHorse(Player player){
		gaitWeight = 0.5f;
		gaitAniSpeed = 1f;
		horseBehaviour.ChangeGaitByRiding (gaitWeight, gaitAniSpeed);
		actualMovementSpeedMultiplier = gaitAniSpeed;
		ui.speedBar.fillAmount = gaitWeight;
		Debug.Log ("mount horse: " + gaitAniSpeed + ", new gait weight: " + gaitWeight + ", actualSpeedMod: " + actualMovementSpeedMultiplier);
	
	
		ReceivePlayerInput (player, dir.UP);
	
	}

	public void ReceivePlayerInput(Player player, dir input/*, Vector3 playerMovementVector = Vector3.one*/){

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
			if (Time.time - pressedLeftLastTime < gaitChangeTapInterval) {
				horseBehaviour.currentHorseGait = DecreaseGaitByOne ();
			}
			pressedLeftLastTime = Time.time;
			break;
		case dir.RIGHT:
			if (Time.time - pressedRightLastTime < gaitChangeTapInterval) {
				horseBehaviour.currentHorseGait = IncreaseGaitByOne ();
			}
			pressedRightLastTime = Time.time;
			break;
		}

		//gait weight: normalize to get val between 0 and 1
		gaitWeight = (gaitAniSpeed - minAniSpeed) / (maxAniSpeed - minAniSpeed);

		//randomize: if you haven't given any input after a while, the horse might slow down or speed up on its own?
		//depending on its energy, motivation, shyness, surroundings? 

		horseBehaviour.ChangeGaitByRiding (gaitWeight, gaitAniSpeed);
		actualMovementSpeedMultiplier = gaitAniSpeed * gaitAniSpeed * speedAdjustmentModifier;

		Debug.Log ("new gait speed: " + gaitAniSpeed + ", new gait weight: " + gaitWeight + ", actualSpeedMod: " + actualMovementSpeedMultiplier);
		ui.speedBar.fillAmount = gaitWeight;

		//calc a value for both of these at once maybe, find a 'formula' to always have two values that fit together?
	}

	private horseGait IncreaseGaitByOne(){
		horseGait result = horseBehaviour.currentHorseGait + 1;
		if ((int)result > 3) {
			result = horseGait.CANTER;
		}
		Debug.Log ("increase gait, old: " + horseBehaviour.currentHorseGait + " new: " + result); 
		return result;
	}

	private horseGait DecreaseGaitByOne(){
		horseGait result = horseBehaviour.currentHorseGait - 1;
		if ((int)result < 0) {
			result = horseGait.STAND;
		}
		Debug.Log ("increase gait, old: " + horseBehaviour.currentHorseGait + " new: " + result); 
		return result;
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
