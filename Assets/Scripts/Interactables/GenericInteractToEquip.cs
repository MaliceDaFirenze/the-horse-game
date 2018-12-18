using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericInteractToEquip : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.PICK_UP:
			equippable.BeEquipped ();
			player.EquipAnItem (equippable);
			break;
		case actionID.PUT_INTO_POCKET:
			player.PutEquippableIntoInventory (equippable);
			player.ExitInteractableTrigger ();
			break;
		}
	}

	public override List<string> DefineInteraction (Player player)	{

		List<string> result = base.DefineInteraction (player);

		//list can be expanded here

		return result;
	}
}
