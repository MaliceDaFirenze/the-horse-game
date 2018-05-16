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


	public override List<string> DefineInteraction (Player player)	{

		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear();

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.PITCHFORK:
			if (player.currentlyEquippedItem.status == containerStatus.FULL) {
				currentlyRelevantActionIDs.Add (actionID.EMPTY_PITCHFORK);
				result.Add(InteractionStrings.GetInteractionStringById(actionID.EMPTY_PITCHFORK));
			} 
			break;
		case equippableItemID.WHEELBARROW:
			if (player.currentlyEquippedItem.status != containerStatus.EMPTY) {
				currentlyRelevantActionIDs.Add (actionID.EMPTY_WHEELBARROW);
				result.Add(InteractionStrings.GetInteractionStringById(actionID.EMPTY_WHEELBARROW));
			}
			break;
		}

		return result;
	}
}
