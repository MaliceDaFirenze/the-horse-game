﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equippable : MonoBehaviour {

	private Vector3 regularScale;
	public float equippedScaleFactor;
	private Consumable consumable;

	private bool wasInitialized;

	private void Start(){
		Initialize ();
	}

	public void Initialize(){
		if (!wasInitialized) {
			regularScale = transform.localScale;
			wasInitialized = true;
		}
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
