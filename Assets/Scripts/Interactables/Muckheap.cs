using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muckheap : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);
		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.PITCHFORK:
			player.currentlyEquippedItem.status = containerStatus.EMPTY;
			Destroy (player.currentlyEquippedItem.content);
			break;
		case equippableItemID.WHEELBARROW:
			player.currentlyEquippedItem.status = containerStatus.EMPTY;
			player.currentlyEquippedItem.GetComponent<Wheelbarrow> ().Empty ();
			break;
		default:
			break;
		}
	}


	public override List<string> GetInteractionStrings (Player player)	{

		List<string> result = new List<string> ();
		currentlyRelevantActionID = actionID._EMPTYSTRING;

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.PITCHFORK:
			if (player.currentlyEquippedItem.status == containerStatus.FULL) {
				currentlyRelevantActionID = actionID.EMPTY_PITCHFORK;
			} 
			break;
		case equippableItemID.WHEELBARROW:
			if (player.currentlyEquippedItem.status != containerStatus.EMPTY) {
				currentlyRelevantActionID = actionID.EMPTY_WHEELBARROW;
			}
			break;
		}

		result.Add(InteractionStrings.GetInteractionStringById(currentlyRelevantActionID));
		return result;
	}
}
