using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayCart : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS:
			TakeHayBale (player);
			break;
		case equippableItemID.STRAW:
			PutAwayHayBale (player);
			break;
		default:
			break;
		}
	}

	private void TakeHayBale(Player player){
		if (player.currentlyEquippedItem.id == equippableItemID.BAREHANDS) {

			GameObject newHayBale = Instantiate (PrefabManager.instance.hayBale, player.equippedItemPos.position, player.equippedItemPos.rotation) as GameObject;

			Equippable equippable = newHayBale.GetComponent<Equippable> ();
			equippable.Initialize ();

			player.EquipAnItem (equippable);
		}
	}

	private void PutAwayHayBale (Player player){

		GameObject.Destroy (player.currentlyEquippedItem.gameObject);
		player.UnequipEquippedItem ();
	}

	public override List<string> GetInteractionStrings (Player player)	{

		List<string> result = new List<string> ();
		currentlyRelevantActionID = actionID._EMPTYSTRING;
		
		switch (player.currentlyEquippedItem.id) {
	
		case equippableItemID.STRAW:
			currentlyRelevantActionID = actionID.PUT_AWAY_STRAW;
			break;
		}
		result.Add(InteractionStrings.GetInteractionStringById(currentlyRelevantActionID));
		return result;
	}
}
