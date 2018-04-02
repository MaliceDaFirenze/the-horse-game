using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equippable : MonoBehaviour {

	private Vector3 regularScale;
	public float equippedScaleFactor;
	private Consumable consumable;

	public void Initialize(){
		regularScale = transform.localScale;
	}

	public void BeEquipped(){
		consumable = GetComponent<Consumable> ();
		if (consumable != null) {
			consumable.enabled = false;
		}
		transform.localScale *= equippedScaleFactor;
	}

	public void BeDropped(){
		transform.localScale = regularScale;
		consumable = GetComponent<Consumable> ();
		if (consumable != null) {
			consumable.enabled = true;
		}
	}
}
