﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum actionID {
	_EMPTYSTRING,
	NO_SEQUENCE,
	BRUSH_HORSE,
	PET_HORSE,
	FEED_HORSE,
	WATER_HORSE,
	CLEAN_MANURE,
	FILL_BUCKET,
	EMPTY_PITCHFORK,
	EMPTY_WHEELBARROW,
	PUT_AWAY_STRAW,
	HANG_UP_HALTER,
	HANG_UP_LEAD,
	TAKE_HALTER,
	TAKE_LEAD,
	PUT_ON_HALTER,
	PUT_ON_LEAD
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

	public virtual List<string> GetInteractionStrings(Player player){
		List<string> result = new List<string> ();
		result.Add (emptyHandsAction);
		return result;
	}
}
