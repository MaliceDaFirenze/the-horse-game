﻿using System.Collections;
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
	private UI ui;
	public Transform equippedItemPos;
	public Transform dropItemPos;

	private void Start() {
		rb = GetComponent<Rigidbody> ();
		ui = FindObjectOfType<UI> ();
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

			if (Input.GetKey (KeyCode.LeftShift)) {
				speedMultiplier = sprintSpeedMultiplier;
			} else {
				speedMultiplier = 1f;
			}

			//------INTERACTION-------//
			if (Input.GetKeyDown(KeyCode.E) && nearestInteractable != null){
				nearestInteractable.PlayerInteracts (this);
			}

			if (Input.GetKeyDown(KeyCode.F) && currentlyEquippedItem != null){
				//drop currently equipped item
				currentlyEquippedItem.BeDropped();
			}
		}
	}

	private void OnTriggerEnter(Collider trigger){
		nearestInteractable = trigger.GetComponent<Interactable> ();
		if (nearestInteractable != null) {
			ui.ShowInstruction(nearestInteractable);

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
		if (trigger.tag.Equals("BuildingEntrance")){
			trigger.transform.parent.GetComponent<Building> ().PlayerExitsBuildingTrigger ();
		}

		nearestInteractable = null;
		ui.HideInstruction ();
		ui.HideHorseUI ();
	}

	public void EquipAnItem(Equippable equippableItem){
		currentlyEquippedItem = equippableItem;

	}
}
