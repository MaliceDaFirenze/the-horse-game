using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Post : Interactable {

	Horse_Interactable horseTiedHere;

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.TIE_HORSE_TO_POST:
			TieHorseToPost (player);
			break;
		case actionID.TAKE_HORSE_FROM_POST:
			TieHorseToPost (player);
			break;
		}
	}

	private void TieHorseToPost(Player player){
		//only parenting, bc horse on lead is same as horse on post?
		horseTiedHere = player.currentlyEquippedItem.GetComponent<Horse_Interactable>();
		horseTiedHere.TieHorseToPost (player);
	}

	//let horse loose from post (lead keeps hanging here)

	private void TakeHorseFromPost(Player player){
		horseTiedHere.TakeHorseFromPost (player);
		horseTiedHere = null;
	}

	public override List<string> DefineInteraction (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear();

		switch (player.currentlyEquippedItem.id) {

		case equippableItemID.HORSE_ON_LEAD:
			currentlyRelevantActionIDs.Add(actionID.TIE_HORSE_TO_POST);
			result.Add(InteractionStrings.GetInteractionStringById(actionID.TIE_HORSE_TO_POST));
			break;
		case equippableItemID.BAREHANDS:
			if (horseTiedHere != null) {
				currentlyRelevantActionIDs.Add(actionID.TIE_HORSE_TO_POST);
				result.Add(InteractionStrings.GetInteractionStringById(actionID.TIE_HORSE_TO_POST));
			}
			break;
		}

		return result;

	}
}
