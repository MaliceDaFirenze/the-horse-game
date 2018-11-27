using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum playerMovementSet{
	WALKING,
	RIDING
	//alternatively: grounded, mounted... more? carriages eventually?
}

public class Player : MonoBehaviour {

	public bool allowPlayerInput;

	//Interaction
	public Interactable nearestInteractable;
	public Horse nearestHorse;
	public Equippable currentlyEquippedItem;


	//Movement
	private float speed = 3f;//30f;
	private float sprintSpeedMultiplier = 1.8f;
	private float speedMultiplier = 1f;
	private float defaultMaximumTurnRate = 500;
	private float maximumTurnRate;
	private Vector3 newMovementVector = new Vector3(0,0,0);
	private playerMovementSet currentMovementSet = playerMovementSet.WALKING;
	private Vector3 previousMovementVector; public Vector3 PreviousMovementVector{ get{return previousMovementVector;} }
	private Vector3 movementVectorInput;
	private float fullInputVectorMagnitude;


	//References
	public Transform equippedItemPos;
	public Transform dropItemPos;
	private Equippable playerHands;
	public Transform playerModel; //to parent to horse pos for riding

	//NavmeshMovement
	public Transform destinationOverride;
	public Transform debugSphere;
	public float movementVectorScale;
	private Vector3 destination;
	//private NavMeshAgent navMeshAgent;

	//Physics
	private Rigidbody rb;

	//character
	public Character characterId;
	public Sprite portrait;

	private void Start() {
		//navMeshAgent = GetComponent<NavMeshAgent> ();
		playerHands = GetComponent<Equippable> ();
		currentlyEquippedItem = playerHands;
		rb = GetComponent<Rigidbody> ();
		maximumTurnRate = defaultMaximumTurnRate;

	}

	private void Update() {

		if (allowPlayerInput) {
            //---------MOVEMENT---------//
            //GetAxis returns value between -1 and 1
            movementVectorInput.x = Input.GetAxisRaw ("Horizontal");
            movementVectorInput.z = Input.GetAxisRaw ("Vertical");

            if(currentMovementSet == playerMovementSet.WALKING) {
                newMovementVector = getMovementVector(movementVectorInput);
            } else if(currentMovementSet == playerMovementSet.RIDING) {
                float desiredTurnRate;
				if (nearestHorse.horseBehavior.isAvoidingCollider) {
					desiredTurnRate = maximumTurnRate * (nearestHorse.horseBehavior.shouldAvoidInPositiveDirection? -1.0f:1.0f);
                } else {
                    desiredTurnRate = movementVectorInput.x * maximumTurnRate;
                }
                newMovementVector = getMovementVector(desiredTurnRate);
            }

            destination = transform.position + newMovementVector;
			if (newMovementVector.magnitude > 0){
				transform.LookAt(new Vector3(destination.x, transform.position.y, destination.z));
			}
			rb.MovePosition (rb.position + newMovementVector);

			if (currentMovementSet == playerMovementSet.WALKING) {
				if (currentlyEquippedItem != null && currentlyEquippedItem.preventSprintingWhileEquipped) {
					speedMultiplier = currentlyEquippedItem.playerSpeedModifier;
				} else if (Input.GetKey (KeyCode.LeftShift)) {
					speedMultiplier = sprintSpeedMultiplier;
				} else {
					speedMultiplier = 1f;
				}

				if (currentlyEquippedItem != null && currentlyEquippedItem.id == equippableItemID.HORSE_ON_LEAD) {
					//Debug.Log ("movement magnitude: " + newMovementVector.magnitude);

					if (nearestHorse == null) {
						nearestHorse = currentlyEquippedItem.GetComponent<Horse> ();
					}

					if (newMovementVector.magnitude > 0) {
						if (Input.GetKey (KeyCode.LeftShift)) {
							nearestHorse.horseBehavior.currentHorseGait = horseGait.TROT;
						} else {
							nearestHorse.horseBehavior.currentHorseGait = horseGait.WALK;
						}
					} else {
						nearestHorse.horseBehavior.currentHorseGait = horseGait.STAND;
					}
				}

				//------INTERACTION-------//
				if (Input.GetKeyDown (KeyCode.E) && nearestInteractable != null && nearestInteractable.arrowInputRequired == null) {
					nearestInteractable.PlayerInteracts (this);
					UI.instance.ShowInstruction (nearestInteractable, this);
				}

				if (nearestInteractable != null && nearestInteractable.arrowInputRequired != null) {
					if (Input.GetKeyDown (KeyCode.LeftArrow)) {
						nearestInteractable.PlayerPressesArrow (this, dir.LEFT);
					} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
						nearestInteractable.PlayerPressesArrow (this, dir.DOWN);
					} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
						nearestInteractable.PlayerPressesArrow (this, dir.RIGHT);
					} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
						nearestInteractable.PlayerPressesArrow (this, dir.UP);
					}
				}

				if (nearestInteractable != null && nearestInteractable.currentlyRelevantActionIDs.Count > 1) {
					if (Input.GetKeyDown (KeyCode.R)) {
						++nearestInteractable.selectedInteractionIndex;
						if (nearestInteractable.selectedInteractionIndex == nearestInteractable.currentlyRelevantActionIDs.Count) {
							nearestInteractable.selectedInteractionIndex = 0;
						}
						UI.instance.ShowInstruction (nearestInteractable, this);
					}
				}
			}

			if (Input.GetKeyDown(KeyCode.F) && currentlyEquippedItem != playerHands){
				DropEquippedItem();
			}

			//------RIDING-------//
			if (currentMovementSet == playerMovementSet.RIDING) {

				speedMultiplier = nearestHorse.horseRidingBehavior.actualMovementSpeedMultiplier;
				nearestHorse.horseRidingBehavior.currentTotalMovementSpeed = speed * speedMultiplier;
                
                    //Debug.Log("SPEED: " + speed * speedMultiplier + " GOTO " + newMovementVector/Time.deltaTime);

				if (nearestHorse.horseBehavior.currentHorseGait != horseGait.STAND && newMovementVector.magnitude == 0) {
					nearestHorse.horseBehavior.currentHorseGait = horseGait.STAND;
				}

				if (Input.GetKeyDown (KeyCode.Q)) {
					nearestHorse.horseRidingBehavior.ReceivePlayerInput (this, dir.LEFT);
				} else if (Input.GetKeyDown (KeyCode.S)) {
					nearestHorse.horseRidingBehavior.ReceivePlayerInput (this, dir.DOWN);
				} else if (Input.GetKeyDown (KeyCode.E)) {
					nearestHorse.horseRidingBehavior.ReceivePlayerInput (this, dir.RIGHT);
				} else if (Input.GetKeyDown (KeyCode.W)) {
					nearestHorse.horseRidingBehavior.ReceivePlayerInput (this, dir.UP);
				} 

				if (previousMovementVector.magnitude > 0 && newMovementVector.magnitude == 0/*&& !keepHorseMoving*/) { 
					nearestHorse.horseRidingBehavior.ReceivePlayerInput (this, dir.DOWN, true);
				//	ridingHorse.horseBehaviour.currentHorseGait = horseGait.STAND;
				} else if (previousMovementVector.magnitude == 0 && newMovementVector.magnitude > 0) {
					Debug.Log ("horse was standing, starts moving now");
					nearestHorse.horseBehavior.currentHorseGait = horseGait.WALK;
					nearestHorse.horseRidingBehavior.ReceivePlayerInput (this, dir.UP, true);
				}
			}

			previousMovementVector = newMovementVector;
		}
	}

    private Vector3 getMovementVector (Vector3 input){
        float turnAngle = Vector3.SignedAngle (transform.forward, input, transform.up); // Should be <= 180 deg
        float turnRate = turnAngle / Time.deltaTime;
        // If resulting turnRate is too large, reduce it
        if (Mathf.Abs (turnRate) > maximumTurnRate) {
            turnRate = Mathf.Sign (turnRate) * maximumTurnRate;
            turnAngle = turnRate * Time.deltaTime;
            input = Quaternion.AngleAxis (turnAngle, transform.up) * transform.forward;
        } 
        Vector3 movementVector = input.normalized * speed * speedMultiplier * Time.deltaTime;
        return movementVector;
    }

    private Vector3 getMovementVector (float turnRate){
        // If turnRate is too large, reduce it
        if (Mathf.Abs (turnRate) > maximumTurnRate) {
            turnRate = Mathf.Sign (turnRate) * maximumTurnRate;
        } 
        float turnAngle = turnRate * Time.deltaTime;
        Vector3 direction = Quaternion.AngleAxis (turnAngle, transform.up) * transform.forward;
        Vector3 movementVector = direction.normalized * speed * speedMultiplier * Time.deltaTime;
        return movementVector;
    }

    private void OnTriggerEnter(Collider trigger){
		Debug.Log ("enter trigger: " + trigger + ", collider is trigger: " + trigger.isTrigger);

		nearestInteractable = trigger.GetComponent<Interactable> ();
		if (nearestInteractable != null) {
			nearestInteractable.nextArrowIndexToInput = 0;
			UI.instance.ShowInstruction(nearestInteractable, this);

			if (nearestInteractable.GetComponent<Horse> () != null) {
				nearestHorse = nearestInteractable.GetComponent<Horse> ();
				UI.instance.ShowHorseUI (nearestHorse.horseStats);
			}
		}

		if (trigger.tag.Equals("BuildingEntrance")){
			trigger.transform.parent.GetComponent<Building> ().PlayerEntersBuildingTrigger ();
		}
	}

	private void OnTriggerExit(Collider trigger){

		Debug.Log ("exit trigger: " + trigger + ", collider is trigger: " + trigger.isTrigger);

		if (trigger.tag.Equals("BuildingEntrance")){
			trigger.transform.parent.GetComponent<Building> ().PlayerExitsBuildingTrigger ();
		}

		if (nearestInteractable != null) {
			ExitInteractableTrigger ();
		}

		UI.instance.HideHorseUI ();
	}

	public void ExitInteractableTrigger(){
		nearestInteractable = null;
		if (!UI.instance.arrowCompletionFXInProgress) {
			UI.instance.HideInstruction ();
		}
	}

	private void DropEquippedItem(){
		Debug.Log ("drop item: " + currentlyEquippedItem.id);
		if (currentlyEquippedItem.id != equippableItemID.HORSE_ON_LEAD && currentlyEquippedItem.id != equippableItemID.HORSE_MOUNTED) { 
			currentlyEquippedItem.transform.position = new Vector3(dropItemPos.position.x, dropItemPos.position.y + currentlyEquippedItem.dropPosYOffset, dropItemPos.position.z);
		} 
		currentlyEquippedItem.transform.SetParent (null);

		currentlyEquippedItem.BeDropped();

		if (currentlyEquippedItem.id == equippableItemID.HORSE_ON_LEAD) {
			Horse_Interactable horseInt = currentlyEquippedItem.GetComponent<Horse_Interactable> ();
			if (horseInt.headGear.type == horseGearType.HALTER) {
				horseInt.LeadIsDropped (this);
			} else if (horseInt.headGear.type == horseGearType.BRIDLE) {
				horseInt.StopLeadingHorseByReins (this);
			}
		} else if (currentlyEquippedItem.id == equippableItemID.HORSE_MOUNTED) {
			Horse_Interactable horseInt = currentlyEquippedItem.GetComponent<Horse_Interactable> ();
			horseInt.Dismount (this);
			currentMovementSet = playerMovementSet.WALKING;
		}

		currentlyEquippedItem = playerHands;
	}

	public void UnequipEquippedItem(){
		currentlyEquippedItem.transform.SetParent (null);
		currentlyEquippedItem = playerHands;
		maximumTurnRate = defaultMaximumTurnRate;
	}

	public void MountHorse(Horse_RidingBehavior mountedHorse){
		currentMovementSet = playerMovementSet.RIDING;
		nearestHorse = mountedHorse.horse;
	}

	public void EquipAnItem(Equippable equippableItem, bool moveItemToPlayer = true, Transform overwriteTransform = null){ //TODO Y U NO WORK? Transform is not passed. test again at some other point?

		//Debug.Log ("equip an item with params: equippableItem " + equippableItem.name + ", moveItemToPlayer: " + moveItemToPlayer + ", overwriteTransform: " + overwriteTransform);
		
		equippableItem.BeEquipped (true);
		currentlyEquippedItem = equippableItem;

		if (currentlyEquippedItem.overrideTurnRate != -1) {
			maximumTurnRate = currentlyEquippedItem.overrideTurnRate;
		}

		if (overwriteTransform != null) {
			Debug.Log ("overwrite transform, parent " + overwriteTransform.name + " to equippeditemPos");
			overwriteTransform.SetParent (equippedItemPos, true);
		} else {
			currentlyEquippedItem.transform.SetParent (equippedItemPos, true);
		}

		if (moveItemToPlayer) {
			currentlyEquippedItem.transform.position = equippedItemPos.position;
			currentlyEquippedItem.transform.localEulerAngles = currentlyEquippedItem.equippedRotation;
			currentlyEquippedItem.transform.localPosition = equippableItem.equippedOffset;
		}
	}
}
