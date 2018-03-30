using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody rb;

	private float speed = 1.5f;
	private Vector3 newMovementVector = new Vector3(0,0,0);

	private void Start() {
		rb = GetComponent<Rigidbody> ();
	}

	private void Update() {
		//GetAxis returns value between -1 and 1
		newMovementVector.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
		newMovementVector.z = Input.GetAxis("Vertical") * speed * Time.deltaTime;

		rb.MovePosition (rb.position += newMovementVector);
		if (newMovementVector.x != 0 || newMovementVector.z != 0) {
			rb.MoveRotation (Quaternion.LookRotation (newMovementVector * 100));
		} else {
			rb.velocity = Vector3.zero;
		}
	}

	private void OnTriggerEnter(Collider trigger){
		if (trigger.tag.Equals("BuildingEntrance")){
			trigger.transform.parent.GetComponent<Building> ().PlayerEntersEntranceTrigger ();
		}
	}
}
