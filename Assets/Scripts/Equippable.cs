using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum equippableItemID{
	BAREHANDS,
	STRAW,
	WATERBUCKET,
	BRUSH,
	PITCHFORK
}

public class Equippable : MonoBehaviour {

	private Vector3 regularScale;
	public float equippedScaleFactor = 1;
	private Consumable consumable;

	public equippableItemID id;

	private bool wasInitialized;

	private Collider[] allColliders = new Collider[0];

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

		EnableAllColliders (false);

		transform.localScale *= equippedScaleFactor;
	}

	public void BeDropped(){
		transform.localScale = regularScale;
		EnableAllColliders (true);
		consumable = GetComponent<Consumable> ();
		if (consumable != null) {
			consumable.enabled = true;
		}
	}

	private void EnableAllColliders (bool enable){
		if (allColliders.Length == 0) {
			allColliders = GetComponentsInChildren<Collider> ();
		}
		for (int i = 0; i < allColliders.Length; ++i) {
			allColliders [i].enabled = enable;
		}
	}
}
