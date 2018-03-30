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
	private Vector3 newMovementVector = new Vector3(0,0,0);

	private void Start() {
		rb = GetComponent<Rigidbody> ();
	}

	private void Update() {

		//---------MOVEMENT---------//

		//GetAxis returns value between -1 and 1
		newMovementVector.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
		newMovementVector.z = Input.GetAxis("Vertical") * speed * Time.deltaTime;

		rb.MovePosition (rb.position += newMovementVector);
		if (newMovementVector.x != 0 || newMovementVector.z != 0) {
			rb.MoveRotation (Quaternion.LookRotation (newMovementVector * 100));
		} else {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}

		//------INTERACTION-------//
		if (Input.GetKeyDown(KeyCode.E) && nearestInteractable != null){
			nearestInteractable.PlayerInteracts ();
		}
	}

	private void OnTriggerEnter(Collider trigger){
		nearestInteractable = trigger.GetComponent<Interactable> ();

		if (trigger.tag.Equals("BuildingEntrance")){
			trigger.transform.parent.GetComponent<Building> ().PlayerEntersBuildingTrigger ();
		}
	}

	private void OnTriggerExit(Collider trigger){
		if (trigger.tag.Equals("BuildingEntrance")){
			trigger.transform.parent.GetComponent<Building> ().PlayerExitsBuildingTrigger ();
		}
	}
}
