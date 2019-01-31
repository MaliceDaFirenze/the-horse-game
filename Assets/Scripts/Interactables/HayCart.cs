using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayCart : Interactable {

	//for current implementation, it only really makes sense for one haycart and one strawcart to exist in the scene. Otherwise, the saving/loading breaks

	public equippableItemID fillType;
	public Renderer hayPileRenderer;
	public int currentUnits;

	public void InitCartFromSave(int newFillAmount){
		currentUnits = newFillAmount;

		if (currentUnits > 0) {
			hayPileRenderer.enabled = true;
		}
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

			if (currentUnits == 0) {
				hayPileRenderer.enabled = false;
			}
		}
	}

	private void PutAwayHayBale (Player player){
		++currentUnits;
		GameObject.Destroy (player.currentlyEquippedItem.gameObject);
		player.UnequipEquippedItem ();

		if (currentUnits > 0 && !hayPileRenderer.enabled) {
			hayPileRenderer.enabled = true;
		}
	}

	public override List<string> DefineInteraction (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear ();

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS:
			if (currentUnits > 0) {
				currentlyRelevantActionIDs.Add (actionID.TAKE_STRAW);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_STRAW) + " (" + currentUnits + ")");
			}
			break;
		case equippableItemID.STRAW:
			currentlyRelevantActionIDs.Add(actionID.PUT_AWAY_STRAW);
			result.Add(InteractionStrings.GetInteractionStringById(actionID.PUT_AWAY_STRAW));
			break;
		}
		return result;
	}
}
