using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	//Interaction
	public Interactable nearestInteractable;

	//Physics
	private Rigidbody rb;

	//Movement
	private float speed = 1.5f;
	private float sprintSpeedMultiplier = 1.8f;
	private float speedMultiplier;
	private Vector3 newMovementVector = new Vector3(0,0,0);

	//References
	private UI ui;

	private void Start() {
		rb = GetComponent<Rigidbody> ();
		ui = FindObjectOfType<UI> ();
	}

	private void Update() {

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
			nearestInteractable.PlayerInteracts ();
		}
	}

	private void OnTriggerEnter(Collider trigger){
		nearestInteractable = trigger.GetComponent<Interactable> ();
		if (nearestInteractable != null) {
			ui.ShowInstruction(nearestInteractable);
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
	}
}
