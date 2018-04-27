﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayCart : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS:
			TakeHayBale (player);
			break;
		case equippableItemID.STRAW:
			PutAwayHayBale (player);
			break;
		default:
			break;
		}
	}

	private void TakeHayBale(Player player){
		if (player.currentlyEquippedItem.id == equippableItemID.BAREHANDS) {

			GameObject newHayBale = Instantiate (PrefabManager.instance.hayBale, player.equippedItemPos.position, player.equippedItemPos.rotation) as GameObject;

			Equippable equippable = newHayBale.GetComponent<Equippable> ();
			equippable.Initialize ();

			player.EquipAnItem (equippable);
		}
	}

	private void PutAwayHayBale (Player player){

		GameObject.Destroy (player.currentlyEquippedItem.gameObject);
		player.UnequipEquippedItem ();
	}

	public override string GetInteractionString (Player player)	{
		
		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS: 
			return emptyHandsAction;
		case equippableItemID.STRAW:
			currentlyRelevantActionID = actionID.PUT_AWAY_STRAW;
			return InteractionStrings.GetInteractionStringById (currentlyRelevantActionID);
		default: 
			return "";
		}
	}
}
