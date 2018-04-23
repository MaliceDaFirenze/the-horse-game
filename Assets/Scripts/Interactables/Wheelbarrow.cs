using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheelbarrow : Interactable {

	public Transform[] contentPositions;
	private int contentPosIndex = 0;

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS:
			GetComponent<Equippable> ().BeEquipped ();
			player.EquipAnItem (equippable);
			break;
		case equippableItemID.PITCHFORK:
			if (player.currentlyEquippedItem.status == equippableStatus.FULL && contentPosIndex < contentPositions.Length) {
				player.currentlyEquippedItem.content.transform.SetParent (transform, true);
				player.currentlyEquippedItem.content.transform.position = contentPositions[contentPosIndex].position;
				++contentPosIndex;
				player.currentlyEquippedItem.content.GetComponent<Collider> ().enabled = false;
				player.currentlyEquippedItem.content = null;
				player.currentlyEquippedItem.status = equippableStatus.EMPTY;
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
		default: 
			return "";
		}
	}
}
