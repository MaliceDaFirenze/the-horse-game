using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muckheap : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);
		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.PITCHFORK:
			player.currentlyEquippedItem.status = equippableStatus.EMPTY;
			Destroy (player.currentlyEquippedItem.content);
			break;
		case equippableItemID.WHEELBARROW:
			player.currentlyEquippedItem.status = equippableStatus.EMPTY;
			player.currentlyEquippedItem.GetComponent<Wheelbarrow> ().Empty ();
			break;
		default:
			break;
		}
	}


	public override string GetInteractionString (Player player)	{
		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS: 
			return emptyHandsAction;
		case equippableItemID.PITCHFORK:
			if (player.currentlyEquippedItem.status == equippableStatus.FULL) {
				currentlyRelevantActionID = actionID.EMPTY_PITCHFORK;
				return InteractionStrings.GetInteractionStringById (currentlyRelevantActionID);
			} else {
				return "";
			}
		case equippableItemID.WHEELBARROW:
			if (player.currentlyEquippedItem.status != equippableStatus.EMPTY) {
				currentlyRelevantActionID = actionID.EMPTY_WHEELBARROW;
				return InteractionStrings.GetInteractionStringById (currentlyRelevantActionID);
			} else {
				return "";
			}
		default: 
			return "";
		}
	}
}
