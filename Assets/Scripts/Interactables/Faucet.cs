using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faucet : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		Debug.Log ("interact with faucet, item: " + player.currentlyEquippedItem.id);

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.WATERBUCKET:
			WaterBucket_Consumable bucket = player.currentlyEquippedItem.GetComponent<WaterBucket_Consumable> ();
			bucket.remainingNeedValue = bucket.totalNeedValue;
			bucket.UpdateValue ();
			break;
		default:
			break;
		}
	}

	public override List<string> GetInteractionStrings (Player player)	{
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
