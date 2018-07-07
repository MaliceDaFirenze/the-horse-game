using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum equippableItemID{
	BAREHANDS,
	STRAW,
	WATERBUCKET,
	BRUSH,
	PITCHFORK,
	WHEELBARROW,
	HALTER,
	LEAD,
	HORSE_ON_LEAD,
	HALTER_WITH_LEAD,
	SADDLE_WITH_PAD	
}

public enum containerStatus{
	EMPTY,
	PARTIALFULL,
	FULL
}

public class Equippable : MonoBehaviour {

	private Vector3 regularScale;
	public float equippedScaleFactor = 1;
	public float dropPosYOffset;
	public Vector3 equippedOffset;
	public float playerSpeedModifier = 1; //player is slower when they carry this
	public bool preventSprintingWhileEquipped = true;
	public Vector3 equippedRotation;
	private Consumable consumable;

	public equippableItemID id;

	private bool wasInitialized;

	private Collider[] allColliders = new Collider[0];

	public containerStatus status;
	public Transform fillNullPos;
	public GameObject content;

	private void Start(){
		Initialize ();
	}

	public void Initialize(){
		if (!wasInitialized) {
			regularScale = transform.localScale;
			wasInitialized = true;
		}
	}

	public virtual void BeEquipped(){
		consumable = GetComponent<Consumable> ();
		if (consumable != null) {
			consumable.Initialize ();
			consumable.enabled = false;
		}

		EnableAllColliders (false);

		transform.localScale *= equippedScaleFactor;
	}

	public virtual void BeDropped(){

		transform.localScale = regularScale;
		EnableAllColliders (true);
		consumable = GetComponent<Consumable> ();
		if (consumable != null) {
			consumable.enabled = true;
		}
	}

	public void EnableAllColliders (bool enable, bool forceUpdateCollidersList = false){
		Debug.Log ("set all colliders on " + name + " to enabled == " + enable);

		//if (allColliders.Length == 0 || forceUpdateCollidersList) {
			allColliders = GetComponentsInChildren<Collider> ();
		//}
		for (int i = 0; i < allColliders.Length; ++i) {
			allColliders [i].enabled = enable;
		}
	}
}
