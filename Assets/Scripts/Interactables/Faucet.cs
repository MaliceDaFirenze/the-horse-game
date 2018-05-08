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

		currentlyRelevantActionID = actionID._EMPTYSTRING;

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.WATERBUCKET:
			currentlyRelevantActionID = actionID.FILL_BUCKET;
			break;
		}

		result.Add(InteractionStrings.GetInteractionStringById(currentlyRelevantActionID));
		return result;

	}
}
