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
	public Horse_Stats nearestHorse;
	public Horse_Behavior leadingHorse;
	public Horse_Mounted ridingHorse;
	public Equippable currentlyEquippedItem;

	//Physics
	private NavMeshAgent navMeshAgent;

	//Movement
	private float speed = 30f;
	private float sprintSpeedMultiplier = 1.8f;
	private float speedMultiplier = 1f;
	private Vector3 newMovementVector = new Vector3(0,0,0);
	private playerMovementSet currentMovementSet = playerMovementSet.WALKING;

	//References
	public UI ui;
	public Transform equippedItemPos;
	public Transform dropItemPos;
	private Equippable playerHands;
	public Transform playerModel; //to parent to horse pos for riding

	private void Start() {
		navMeshAgent = GetComponent<NavMeshAgent> ();
		ui = FindObjectOfType<UI> ();
		playerHands = GetComponent<Equippable> ();
		currentlyEquippedItem = playerHands;
	}

	public Transform destinationOverride;
	public Transform debugSphere;
	public float movementVectorScale;
	private Vector3 destination;

	private void Update() {

		if (allowPlayerInput) {
			//---------MOVEMENT---------//
			//GetAxis returns value between -1 and 1
			newMovementVector.x = Input.GetAxis("Horizontal") * speed * speedMultiplier * Time.deltaTime;
			newMovementVector.z = Input.GetAxis("Vertical") * speed * speedMultiplier * Time.deltaTime;

			if (destinationOverride != null) {
				destination = destinationOverride.position;
			} else {
				destination = transform.position + newMovementVector;
			}

			navMeshAgent.SetDestination (destination);

			transform.LookAt(new Vector3(destination.x, transform.position.y, destination.z));

			if (!navMeshAgent.hasPath && newMovementVector.magnitude != 0) {
				//the player is standing and moving against an edge of the navmesh
				//I need to check if there are offmesh links available nearby

				Vector3 overrideMoVe;
				Debug.Log ("no path, but trying to move");
		
				overrideMoVe = newMovementVector * movementVectorScale;
				debugSphere.transform.position = transform.position + overrideMoVe;
				navMeshAgent.SetDestination (transform.position + overrideMoVe);
			}


			if (currentMovementSet == playerMovementSet.WALKING) {
				if (currentlyEquippedItem != null && currentlyEquippedItem.playerSpeedModifier != 1 && currentlyEquippedItem.preventSprintingWhileEquipped) {
					speedMultiplier = currentlyEquippedItem.playerSpeedModifier;
				} else if (Input.GetKey (KeyCode.LeftShift)) {
					speedMultiplier = sprintSpeedMultiplier;
				} else {
					speedMultiplier = 1f;
				}

				if (currentlyEquippedItem != null && (currentlyEquippedItem.id == equippableItemID.HORSE_ON_LEAD || currentlyEquippedItem.id == equippableItemID.HORSE_MOUNTED)) {
					//Debug.Log ("movement magnitude: " + newMovementVector.magnitude);

					if (leadingHorse == null) {
						leadingHorse = currentlyEquippedItem.GetComponent<Horse_Behavior> ();
					}

					if (newMovementVector.magnitude > 0) {
						if (Input.GetKey (KeyCode.LeftShift)) {
							leadingHorse.currentHorseGait = horseGait.TROT;
						} else {
							leadingHorse.currentHorseGait = horseGait.WALK;
						}
					} else {
						leadingHorse.currentHorseGait = horseGait.STAND;
					}
				}

				//------INTERACTION-------//
				if (Input.GetKeyDown (KeyCode.E) && nearestInteractable != null && nearestInteractable.arrowInputRequired == null) {
					nearestInteractable.PlayerInteracts (this);
					ui.ShowInstruction (nearestInteractable, this);
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
						ui.ShowInstruction (nearestInteractable, this);
					}
				}
			}

			if (Input.GetKeyDown(KeyCode.F) && currentlyEquippedItem != playerHands){
				DropEquippedItem();
			}

			//------RIDING-------//
			if (currentMovementSet == playerMovementSet.RIDING) {

				speedMultiplier = ridingHorse.actualMovementSpeedMultiplier;

				if (ridingHorse.horseBehaviour.currentHorseGait == horseGait.STAND && newMovementVector.magnitude > 0) {
					ridingHorse.horseBehaviour.currentHorseGait = horseGait.WALK;
				} else if (ridingHorse.horseBehaviour.currentHorseGait != horseGait.STAND && newMovementVector.magnitude == 0) {
					ridingHorse.horseBehaviour.currentHorseGait = horseGait.STAND;
				}

				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					ridingHorse.ReceivePlayerInput (this, dir.LEFT);
				} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
					ridingHorse.ReceivePlayerInput (this, dir.DOWN);
				} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
					ridingHorse.ReceivePlayerInput (this, dir.RIGHT);
				} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
					ridingHorse.ReceivePlayerInput (this, dir.UP);
				} 
			}
		}
	}

	private void OnTriggerEnter(Collider trigger){
		Debug.Log ("enter trigger: " + trigger + ", collider is trigger: " + trigger.isTrigger);

		nearestInteractable = trigger.GetComponent<Interactable> ();
		if (nearestInteractable != null) {
			nearestInteractable.nextArrowIndexToInput = 0;
			ui.ShowInstruction(nearestInteractable, this);

			nearestHorse = nearestInteractable.GetComponent<Horse_Stats> ();
			if (nearestHorse != null) {
				ui.ShowHorseUI (nearestHorse);
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

		ui.HideHorseUI ();
	}

	public void ExitInteractableTrigger(){
		nearestInteractable = null;
		if (!ui.arrowCompletionFXInProgress) {
			ui.HideInstruction ();
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
	}

	public void MountHorse(Horse_Mounted mountedHorse){
		currentMovementSet = playerMovementSet.RIDING;
		ridingHorse = mountedHorse;
	}

	public void EquipAnItem(Equippable equippableItem, bool moveItemToPlayer = true, Transform overwriteTransform = null){ //TODO Y U NO WORK? Transform is not passed. test again at some other point?

		//Debug.Log ("equip an item with params: equippableItem " + equippableItem.name + ", moveItemToPlayer: " + moveItemToPlayer + ", overwriteTransform: " + overwriteTransform);
		
		equippableItem.BeEquipped (true);
		currentlyEquippedItem = equippableItem;

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
