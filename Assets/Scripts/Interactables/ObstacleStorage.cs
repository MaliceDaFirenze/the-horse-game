using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleStorage : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);
		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.REARRANGE_OBSTACLES:
			//TODO 
			break;
		}
	}


	public override List<string> DefineInteraction (Player player)	{

		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear();

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS:
			if (player.currentlyEquippedItem.status == containerStatus.FULL) {
				currentlyRelevantActionIDs.Add (actionID.REARRANGE_OBSTACLES);
				result.Add(InteractionStrings.GetInteractionStringById(actionID.REARRANGE_OBSTACLES));
			} 
			break;
		}

		return result;
	}
}
