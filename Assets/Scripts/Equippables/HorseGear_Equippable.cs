using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseGear_Equippable : Equippable {

	public override void BeEquipped(bool overwriteCollidersTo = false){
		base.BeEquipped ();

	}

	public override void BeDropped(){
		base.BeDropped ();
		//be dropped enables all colliders, so I only need to disable irrelevant ones

		switch (id) {
		case equippableItemID.HALTER_WITH_LEAD:
			if (transform.parent != null) {
				//this is child of halter and halter is alone
				GetComponent<SphereCollider> ().enabled = false;
			} else {
				//this has no parent, so halter and lead are ITS children and their colliders need to be disabled
				SphereCollider[] collidersInChildren = GetComponentsInChildren<SphereCollider> ();
				foreach (SphereCollider sc in collidersInChildren) {
					if (sc.transform != transform) {
						sc.enabled = false;
					}
				}
			}
			break;
		case equippableItemID.HALTER:
			if (transform.parent == null) { 
				//this halter is not child of combined, so collider of ITS child (combined) needs to be disabled
				//I think this entire thing is redundant bc I'm already disabling the combined's collider above in this case. 
				//the loops might not be necessary at all if I let the relevant collider disable itself?
				SphereCollider[] collidersInChildren = GetComponentsInChildren<SphereCollider> ();
				foreach (SphereCollider sc in collidersInChildren) {
					if (sc.transform != transform) {
						sc.enabled = false;
					}
				}
			} 
			break;
		case equippableItemID.LEAD:
			if (transform.parent != null) {
				GetComponent<SphereCollider>().enabled = false;
			} 
			break;
		}
	}
}
