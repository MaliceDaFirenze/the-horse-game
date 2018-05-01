using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : Interactable {

	public containerStatus hookStatus;
	public Equippable content;

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS:
			if (hookStatus == containerStatus.FULL) {
				//take halter or lead or whatever hangs here
			}
			break;
		case equippableItemID.HALTER:
			if (hookStatus == containerStatus.EMPTY) {
				//hang up
			}
			break;
		case equippableItemID.LEAD:
			if (hookStatus == containerStatus.EMPTY) {
				//hang up
			}
			break;
		default:
			break;
		}
	}

	public override string GetInteractionString (Player player)	{

		currentlyRelevantActionID = actionID._EMPTYSTRING;

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS: 
			if (hookStatus == containerStatus.FULL) {
				if (content.id == equippableItemID.HALTER) {
					currentlyRelevantActionID = actionID.TAKE_HALTER;
				} else if (content.id == equippableItemID.LEAD) {
					currentlyRelevantActionID = actionID.TAKE_LEAD;
				} 
			}
			break;
		case equippableItemID.HALTER:
			if (hookStatus == containerStatus.EMPTY) {
				currentlyRelevantActionID = actionID.HANG_UP_HALTER;
			}
			break;
		case equippableItemID.LEAD:
			if (hookStatus == containerStatus.EMPTY) {
				currentlyRelevantActionID = actionID.HANG_UP_LEAD;
			}
			break;
		}

		return InteractionStrings.GetInteractionStringById (currentlyRelevantActionID);

	}
}
