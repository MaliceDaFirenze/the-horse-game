using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreCase : Interactable {

	Horse_Interactable horseTiedHere;

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.OPEN_SHOP:
			break;
		}
	}

	public override List<string> DefineInteraction (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear();

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS:
			currentlyRelevantActionIDs.Add(actionID.OPEN_SHOP);
			result.Add(InteractionStrings.GetInteractionStringById(actionID.OPEN_SHOP));
			break;
		}

		return result;

	}
}
