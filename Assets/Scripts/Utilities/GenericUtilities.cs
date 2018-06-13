﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericUtilities : MonoBehaviour {

	public static void EnableAllColliders (Transform target, bool enable){
		Debug.Log ("set all colliders on " + target.name + " to enabled == " + enable);
		Collider[] allColliders = target.GetComponentsInChildren<Collider> ();
		for (int i = 0; i < allColliders.Length; ++i) {
			allColliders [i].enabled = enable;
		}
	}
}
