using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faucet : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.FILL_BUCKET:
			FillBucket (player);
			break;
		}
	}

	private void FillBucket(Player player){
		WaterBucket_Consumable bucket = player.currentlyEquippedItem.GetComponent<WaterBucket_Consumable> ();
		bucket.remainingNeedValue = bucket.totalNeedValue;
		bucket.UpdateValue ();
	}

	public override List<string> DefineInteraction (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear ();

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.WATERBUCKET:
			currentlyRelevantActionIDs.Add(actionID.FILL_BUCKET);
			result.Add(InteractionStrings.GetInteractionStringById(actionID.FILL_BUCKET));
			break;
		}

		return result;

	}
}
