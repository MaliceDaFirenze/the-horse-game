using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : Interactable {

	public containerStatus hookStatus;
	public Equippable content;

	private void Start(){
		//search in children, 'equip' halter or lead if any are there
		content = GetComponentInChildren<Equippable>();
		if (content != null) {
			content.EnableAllColliders (false);
			hookStatus = containerStatus.FULL;
		} else {
			hookStatus = containerStatus.EMPTY;
		}
	}

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS:
			if (hookStatus == containerStatus.FULL) {
				//take halter or lead or whatever hangs here
				hookStatus = containerStatus.EMPTY;
				player.EquipAnItem(content);
				content.BeEquipped ();
				content = null;
			}
			break;
		case equippableItemID.HALTER:
			if (hookStatus == containerStatus.EMPTY) {
				hookStatus = containerStatus.FULL;
				content = player.currentlyEquippedItem;
				player.UnequipEquippedItem ();
				content.transform.SetParent (transform);
				content.transform.localPosition = Vector3.zero;
			}
			break;
		case equippableItemID.LEAD:
			if (hookStatus == containerStatus.EMPTY) {
				hookStatus = containerStatus.FULL;
				content = player.currentlyEquippedItem;
				player.UnequipEquippedItem ();
				content.transform.SetParent (transform);
				content.transform.localPosition = Vector3.zero;
			}
			break;
		default:
			break;
		}
	}

	public override List<string> GetInteractionStrings (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionID = actionID._EMPTYSTRING;

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS: 
			if (hookStatus == containerStatus.FULL) {
				if (content.id == equippableItemID.HALTER) {
					currentlyRelevantActionID = actionID.TAKE_HALTER;
				} else if (content.id == equippableItemID.LEAD) {
					currentlyRelevantActionID = actionID.TAKE_LEAD;
				} 
			}
			break;
		case equippableItemID.HALTER:
			if (hookStatus == containerStatus.EMPTY) {
				currentlyRelevantActionID = actionID.HANG_UP_HALTER;
			}
			break;
		case equippableItemID.LEAD:
			if (hookStatus == containerStatus.EMPTY) {
				currentlyRelevantActionID = actionID.HANG_UP_LEAD;
			}
			break;
		}

		result.Add(InteractionStrings.GetInteractionStringById(currentlyRelevantActionID));
		return result;

	}
}
