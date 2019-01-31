using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreCase : Interactable {

	public HayCart cartToRestock;

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.RESTOCK:
			break;
		}
	}

	private void RestockHayCart(){
	
	}

	public override List<string> DefineInteraction (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear();

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS:
			//NEXT: add different options, like restock 10 for x credits, or fill cart for y credits

			currentlyRelevantActionIDs.Add(actionID.RESTOCK);
			result.Add(InteractionStrings.GetInteractionStringById(actionID.RESTOCK));
			break;
		}

		return result;

	}
}
