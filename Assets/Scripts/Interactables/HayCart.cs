using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayCart : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.TAKE_STRAW:
			TakeHayBale (player);
			break;
		case actionID.PUT_AWAY_STRAW:
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

	public override List<string> DefineInteraction (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear ();

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS:
			currentlyRelevantActionIDs.Add (actionID.TAKE_STRAW);
			result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_STRAW));
			break;
		case equippableItemID.STRAW:
			currentlyRelevantActionIDs.Add(actionID.PUT_AWAY_STRAW);
			result.Add(InteractionStrings.GetInteractionStringById(actionID.PUT_AWAY_STRAW));
			break;
		}
		return result;
	}
}
