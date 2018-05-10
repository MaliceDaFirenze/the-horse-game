using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public bool allowPlayerInput;

	//Interaction
	public Interactable nearestInteractable;
	public Horse_Stats nearestHorse;
	public Equippable currentlyEquippedItem;

	//Physics
	private Rigidbody rb;

	//Movement
	private float speed = 1.5f;
	private float sprintSpeedMultiplier = 1.8f;
	private float speedMultiplier;
	private Vector3 newMovementVector = new Vector3(0,0,0);

	//References
	public UI ui;
	public Transform equippedItemPos;
	public Transform dropItemPos;
	private Equippable playerHands;

	private void Start() {
		rb = GetComponent<Rigidbody> ();
		ui = FindObjectOfType<UI> ();
		playerHands = GetComponent<Equippable> ();
		currentlyEquippedItem = playerHands;
	}

	private void Update() {

		if (allowPlayerInput) {
			//---------MOVEMENT---------//
			//GetAxis returns value between -1 and 1
			newMovementVector.x = Input.GetAxis("Horizontal") * speed * speedMultiplier * Time.deltaTime;
			newMovementVector.z = Input.GetAxis("Vertical") * speed * speedMultiplier * Time.deltaTime;

			rb.MovePosition (rb.position += newMovementVector);
			if (newMovementVector.x != 0 || newMovementVector.z != 0) {
				rb.MoveRotation (Quaternion.LookRotation (newMovementVector * 100));
			} else {
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
			}

			if (currentlyEquippedItem != null && currentlyEquippedItem.playerSpeedModifier != 1) {
				speedMultiplier = currentlyEquippedItem.playerSpeedModifier;
			} else if (Input.GetKey (KeyCode.LeftShift)) {
				speedMultiplier = sprintSpeedMultiplier;
			} else {
				speedMultiplier = 1f;
			}

			//------INTERACTION-------//
			if (Input.GetKeyDown(KeyCode.E) && nearestInteractable != null && nearestInteractable.arrowInputRequired == null){
				nearestInteractable.PlayerInteracts (this);
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

			if (Input.GetKeyDown(KeyCode.F) && currentlyEquippedItem != playerHands){
				DropEquippedItem();
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

		nearestInteractable = null;
		if (!ui.arrowCompletionFXInProgress) {
			ui.HideInstruction ();
		}
		ui.HideHorseUI ();
	}

	private void DropEquippedItem(){
		currentlyEquippedItem.transform.position = new Vector3(dropItemPos.position.x, dropItemPos.position.y + currentlyEquippedItem.dropPosYOffset, dropItemPos.position.z);
		currentlyEquippedItem.transform.SetParent (null);
		currentlyEquippedItem.BeDropped();
		currentlyEquippedItem = playerHands;
	}

	public void UnequipEquippedItem(){
		currentlyEquippedItem = playerHands;
	}

	public void EquipAnItem(Equippable equippableItem){
		equippableItem.BeEquipped ();
		currentlyEquippedItem = equippableItem;
		currentlyEquippedItem.transform.position = equippedItemPos.position;
		currentlyEquippedItem.transform.SetParent (transform, true);
		currentlyEquippedItem.transform.localEulerAngles = currentlyEquippedItem.equippedRotation;
		Debug.Log ("setting " + currentlyEquippedItem.name + " to " + currentlyEquippedItem.equippedRotation + ", rot is " + currentlyEquippedItem.transform.eulerAngles);
		ui.HideInstruction ();

	}
}
