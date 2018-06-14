using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericInteractToEquip : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.PICK_UP:
			GetComponent<Equippable> ().BeEquipped ();
			player.EquipAnItem (equippable);
			break;
		}
	}

	public override List<string> DefineInteraction (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear ();

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS:
			currentlyRelevantActionIDs.Add(actionID.PICK_UP);
			result.Add(InteractionStrings.GetInteractionStringById(actionID.PICK_UP));
			break;
		}

		return result;

	}
}
