using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Defines how the horse reacts to player input while riding.
 */
public class Horse_RidingBehavior : MonoBehaviour {

	//what does this need to do?

	//establish links to behaviour, maybe stats, interactable, player...
	//input can be handled in Player script, but needs to be forwarded to this, where
	//the speed up/down and gait up/down logic is handled

	private float gaitWeight;
	public float gaitAniSpeed;

	public float actualMovementSpeedMultiplier = 1f; //which is used by the player for the actual movement
//	private float speedAdjustmentModifier = 0.5f;
	//private Dictionary<horseGait, float> speedAdjustmentModifierPerGait = new Dictionary<horseGait, float> ();
	private Dictionary<horseGait, float> minSpeedMod = new Dictionary<horseGait, float> ();
	private Dictionary<horseGait, float> maxSpeedMod = new Dictionary<horseGait, float> ();

	private float changeSpeedValueBy;
	private float changeSpeedValueByMin = 0.05f;
	private float changeSpeedValueByMax = 0.2f;

//	private float minAniSpeed = 0.85f;
	//private float maxAniSpeed = 1.7f;

	private Dictionary<horseGait, float> minAniSpeed = new Dictionary<horseGait, float> ();
	private Dictionary<horseGait, float> maxAniSpeed = new Dictionary<horseGait, float> ();

	private HorseRidingUI ui;

	private float pressedLeftLastTime = 0f;
	private float pressedRightLastTime = 0f;
	private float gaitChangeTapInterval = 0.6f;

	public bool ignorePlayerInput;
    public float currentTotalMovementSpeed;

	private void Start(){

		ui = FindObjectOfType<HorseRidingUI> ();

		//Animation Speed
		minAniSpeed.Add (horseGait.STAND, 0.5f);
		minAniSpeed.Add (horseGait.WALK, 0.7f);
		minAniSpeed.Add (horseGait.TROT, 1f);
		minAniSpeed.Add (horseGait.CANTER, 1.1f);


		maxAniSpeed.Add (horseGait.STAND, 1f);
		maxAniSpeed.Add (horseGait.WALK, 2.2f);
		maxAniSpeed.Add (horseGait.TROT, 1.8f);
		maxAniSpeed.Add (horseGait.CANTER, 2.3f);

		//Movement Speed
		minSpeedMod.Add (horseGait.STAND, 0.0f);
		minSpeedMod.Add (horseGait.WALK, 0.25f);
		minSpeedMod.Add (horseGait.TROT, 0.7f);
		minSpeedMod.Add (horseGait.CANTER, 1.3f);

		maxSpeedMod.Add (horseGait.STAND, 0f);
		maxSpeedMod.Add (horseGait.WALK, 1.4f);
		maxSpeedMod.Add (horseGait.TROT, 2.4f);
		maxSpeedMod.Add (horseGait.CANTER, 5.3f);
	}

	private void Update(){
		//only if mounted is active / rider is on horse
		//gradually move speed towards 0.5 and ani speed towards 1 (depending on horse mood / energy / character though)
	
	}

	public void MountHorse(Player player){
		gaitWeight = 0.5f;
		gaitAniSpeed = 1f;
		horseBehaviour.ChangeGaitByRiding (gaitWeight, gaitAniSpeed);
		//actualMovementSpeedMultiplier = minSpeedMod[horseGait.WALK] + gaitWeight * (maxSpeedMod[horseGait.WALK] - minSpeedMod[horseGait.WALK]);
		actualMovementSpeedMultiplier =0;
            ui.speedBar.fillAmount = gaitWeight;
		Debug.Log ("mount horse: speed " + gaitAniSpeed + ", new gait weight: " + gaitWeight + ", actualSpeedMod: " + actualMovementSpeedMultiplier);
	
	}

	public void ReceivePlayerInput(Player player, dir input, bool useMinChangeValue = false/*, Vector3 playerMovementVector = Vector3.one*/){

		if (ignorePlayerInput) {
			return;
		}

		if (useMinChangeValue) {
			changeSpeedValueBy = changeSpeedValueByMin;
		} else {
			changeSpeedValueBy = Random.Range (changeSpeedValueByMin, changeSpeedValueByMax);
		}
		Debug.Log ("player input: " + input + ". changevalueby: " + changeSpeedValueBy + ". BEFORE: new ani speed: " + gaitAniSpeed + ", in gait: " + horseBehaviour.currentHorseGait + ", new gait weight: " + gaitWeight + ", actualSpeedMod: " + actualMovementSpeedMultiplier + ". use min Value: " + useMinChangeValue);

		switch (input) {
		case dir.UP: 
			gaitAniSpeed += changeSpeedValueBy;
			if (gaitAniSpeed > maxAniSpeed[horseBehaviour.currentHorseGait]) {
				gaitAniSpeed = maxAniSpeed[horseBehaviour.currentHorseGait];
			}
			break;
		case dir.DOWN:
			gaitAniSpeed -= changeSpeedValueBy;
			if (gaitAniSpeed < minAniSpeed[horseBehaviour.currentHorseGait]) {
				gaitAniSpeed = minAniSpeed[horseBehaviour.currentHorseGait];
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
		gaitWeight = (gaitAniSpeed - minAniSpeed[horseBehaviour.currentHorseGait]) / (maxAniSpeed[horseBehaviour.currentHorseGait] - minAniSpeed[horseBehaviour.currentHorseGait]);

		//randomize: if you haven't given any input after a while, the horse might slow down or speed up on its own?
		//depending on its energy, motivation, shyness, surroundings? 

		horseBehaviour.ChangeGaitByRiding (gaitWeight, gaitAniSpeed);
		//actualMovementSpeedMultiplier = gaitAniSpeed * gaitAniSpeed * speedAdjustmentModifierPerGait[horseBehaviour.currentHorseGait]; 

		actualMovementSpeedMultiplier = minSpeedMod[horseBehaviour.currentHorseGait] + gaitWeight * (maxSpeedMod[horseBehaviour.currentHorseGait] - minSpeedMod[horseBehaviour.currentHorseGait] );

		//Debug.Log ("new ani speed: " + gaitAniSpeed + ", in gait: " + horseBehaviour.currentHorseGait + ", new gait weight: " + gaitWeight + ", actualSpeedMod: " + actualMovementSpeedMultiplier);
	
		ui.speedBar.fillAmount = gaitWeight;

	}

	public void Stop(){
		Debug.Log ("HORSE STOP");
		horseBehaviour.currentHorseGait = horseGait.STAND;
		horseBehaviour.ChangeGaitByRiding (1f, 1f);
		actualMovementSpeedMultiplier = 0;
		ignorePlayerInput = true;
		StartCoroutine (ResetIgnorePlayer ());
	}

	private IEnumerator ResetIgnorePlayer(){
		yield return new WaitForSeconds (3f);
		ignorePlayerInput = false;
		actualMovementSpeedMultiplier = 1;
		Debug.Log ("HORSE IGNORING STOP");
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
		if (result < horseGait.STAND) {
			result = horseGait.STAND;
		}
		Debug.Log ("decrease gait, old: " + horseBehaviour.currentHorseGait + " new: " + result); 
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
