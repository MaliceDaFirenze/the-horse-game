using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseGear_Equippable : Equippable {

	public override void BeEquipped(){
		base.BeEquipped ();
	}

	public override void BeDropped(){
		base.BeDropped ();
		//be dropped enables all colliders, so I only need to disable irrelevant ones

		Debug.Log ("override BeDropped");

		switch (id) {
		case equippableItemID.HALTER_WITH_LEAD:
			if (transform.parent != null) {
				transform.parent.GetComponent<SphereCollider>().enabled = false;
				Debug.Log ("disable collider on " + transform.parent.name);
			} else {
				//this is child of halter and halter is alone
				GetComponent<SphereCollider>().enabled = false;
				Debug.Log ("disable collider on " + name + "with id " + id);
			}
			break;
		case equippableItemID.HALTER:
			if (transform.parent == null) {
				GetComponent<SphereCollider>().enabled = false;
				Debug.Log ("disable collider on " + name + "with id " + id);
			} 
			break;
		case equippableItemID.LEAD:
			if (transform.parent == null) {
				GetComponent<SphereCollider>().enabled = false;
				Debug.Log ("disable collider on " + name + "with id " + id);
			} 
			break;
		}
	}
}
