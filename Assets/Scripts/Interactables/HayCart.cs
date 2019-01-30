using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayCart : Interactable {

	//for current implementation, it only really makes sense for one haycart and one strawcart to exist in the scene. Otherwise, the saving/loading breaks

	public equippableItemID fillType;
	public int currentUnits;

	public void InitCartFromSave(int newFillAmount){
		currentUnits = newFillAmount;
	}

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.TAKE_STRAW:
			TakeHayBale (player);
			break;
		case actionID.PUT_AWAY_STRAW:
			PutAwayHayBale (player);
			break;
		}
	}

	private void TakeHayBale(Player player){

		//NEXT: when currentunits == 0, hide pile model and don't allow taking more 

		if (player.currentlyEquippedItem.id == equippableItemID.BAREHANDS) {

			GameObject newBale;
			if (fillType == equippableItemID.STRAW) {
				newBale = Instantiate (PrefabManager.instance.strawBale, player.equippedItemPos.position, player.equippedItemPos.rotation) as GameObject;
			
			} else { // if (fillType == equippableItemID.HAY) { is implied but this way newbale is definitely defined
				newBale = Instantiate (PrefabManager.instance.hayBale, player.equippedItemPos.position, player.equippedItemPos.rotation) as GameObject;
			}

			--currentUnits;

			Equippable equippable = newBale.GetComponent<Equippable> ();
			equippable.Initialize ();

			player.EquipAnItem (equippable);
		}
	}

	private void PutAwayHayBale (Player player){
		++currentUnits;
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
