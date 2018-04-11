using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum actionID {
	NO_SEQUENCE,
	BRUSH_HORSE,
	PET_HORSE,
	FEED_HORSE,
	WATER_HORSE,
	CLEAN_MANURE,
	FILL_BUCKET
}

public class Interactable : MonoBehaviour {

	public string emptyHandsAction;
	public Equippable equippable;
	public Consumable consumable;

	public actionID currentlyRelevantActionID; //changes based on equippable, get set on enter int trigger
	public dir[] arrowInputRequired; //changes with actionID

	private void Awake(){
		if (GetComponent<Collider> () == null) {
			Debug.LogWarning ("Interactable " + name + " does not have collider!");
		} else if (!GetComponent<Collider> ().isTrigger) {
			Debug.LogWarning ("Interactable " + name + " collider is not trigger!");
		}

		equippable = GetComponent<Equippable> ();
		consumable = GetComponent<Consumable> ();
	}

	public virtual void PlayerInteracts(Player player){
		Debug.Log ("base interact");
	}

	public virtual void PlayerEntersIntTrigger(){
		
	}

	public virtual string GetInteractionString(Player player){
		return emptyHandsAction;
	}
}
