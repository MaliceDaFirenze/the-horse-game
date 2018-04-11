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

	public override string GetInteractionString (Player player)	{
		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS: 
			return emptyHandsAction;
		case equippableItemID.WATERBUCKET:
			currentlyRelevantActionID = actionID.FILL_BUCKET;
			return "Fill Bucket";
		default: 
			return "";
		}
	}
}
