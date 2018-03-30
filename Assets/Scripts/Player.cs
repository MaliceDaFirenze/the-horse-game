using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody rb;

	private float speed = 2;
	private Vector3 newMovementVector = new Vector3(0,0,0);
	private Vector3 newRotationVector = new Vector3(0,0,0);

	private void Start() {
		rb = GetComponent<Rigidbody> ();
	}

	private void Update() {
		//GetAxis returns value between -1 and 1
		newMovementVector.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
		newMovementVector.z = Input.GetAxis("Vertical") * speed * Time.deltaTime;

		//newRotationVector = newMovementVector - rb.position;
		//Debug.Log ("newRotationVector" + newRotationVector);

		rb.MovePosition (rb.position += newMovementVector);
		//rb.MoveRotation(Quaternion.Euler(newRotationVector));
	}

	private void OnTriggerEnter(Collider trigger){
		if (trigger.tag.Equals("BuildingEntrance")){
			trigger.transform.parent.GetComponent<Building> ().PlayerEntersEntranceTrigger ();
		}
	}
}
