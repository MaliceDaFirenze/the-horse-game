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
	public int nextArrowIndexToInput;

	private void Awake(){
		if (GetComponent<Collider> () == null) {
			Debug.LogWarning ("Interactable " + name + " does not have collider!");
		} else if (!GetComponent<Collider> ().isTrigger) {
			Debug.LogWarning ("Interactable " + name + " collider is not trigger!");
		}

		equippable = GetComponent<Equippable> ();
		consumable = GetComponent<Consumable> ();
	}

	public void PlayerPressesArrow(Player player, dir input){
		if (input == arrowInputRequired [nextArrowIndexToInput]) {
			//Debug.Log ("arrow correct");
			++nextArrowIndexToInput;
			player.ui.UpdateArrows (nextArrowIndexToInput);
			if (arrowInputRequired.Length == nextArrowIndexToInput) {
				PlayerInteracts (player);
				player.ui.ArrowSequenceComplete ();
			}
		} else {
			//Debug.Log ("arrow false");
			nextArrowIndexToInput = 0;
			player.ui.UpdateArrows (nextArrowIndexToInput);
			player.ui.ShakeArrows ();
		}
	}

	public virtual void PlayerInteracts(Player player){
		//Debug.Log ("base interact");
	}

	public virtual void PlayerEntersIntTrigger(){
		nextArrowIndexToInput = 0;
	}

	public virtual string GetInteractionString(Player player){
		return emptyHandsAction;
	}
}
