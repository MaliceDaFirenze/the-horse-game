using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayWithTransform : MonoBehaviour {
	public bool updateRotation;
	public bool updatePosition;
	public Transform target;

	void Update(){
		if (updatePosition) {
			transform.position = target.transform.position;
		}	

		if (updateRotation) {
			transform.rotation = target.transform.rotation;
		}
	}
}
