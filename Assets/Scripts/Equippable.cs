﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum equippableItemID{
	BAREHANDS,
	STRAW,
	WATERBUCKET,
	BRUSH
}

public class Equippable : MonoBehaviour {

	private Vector3 regularScale;
	public float equippedScaleFactor = 1;
	private Consumable consumable;

	public equippableItemID id;

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
			consumable.Initialize ();
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
