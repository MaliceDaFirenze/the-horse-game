using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muckheap : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);
		//how does filling pitchfork work?
		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.PITCHFORK:
			if (player.currentlyEquippedItem.status == equippableStatus.EMPTY) {
				player.currentlyEquippedItem.status = equippableStatus.FULL;
				transform.SetParent (player.currentlyEquippedItem.transform);
				transform.position = player.currentlyEquippedItem.fillNullPos.position;
				EnableAllColliders (false);
				player.currentlyEquippedItem.content = gameObject;
			}
			break;
		default:
			break;
		}
	}


	public override string GetInteractionString (Player player)	{
		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS: 
			return emptyHandsAction;
		case equippableItemID.PITCHFORK:
			if (player.currentlyEquippedItem.status == equippableStatus.FULL) {
				currentlyRelevantActionID = actionID.EMPTY_PITCHFORK;
				return "Empty Pitchfork";
			} else {
				return "";
			}
		case equippableItemID.WHEELBARROW:
			if (player.currentlyEquippedItem.status != equippableStatus.EMPTY) {
				currentlyRelevantActionID = actionID.EMPTY_WHEELBARROW;
				return "Empty Wheelbarrow";
			} else {
				return "";
			}
		default: 
			return "";
		}
	}

	public void EnableAllColliders (bool enable){
		Collider[] allColliders = GetComponentsInChildren<Collider> ();
		for (int i = 0; i < allColliders.Length; ++i) {
			allColliders [i].enabled = enable;
		}
	}
}
