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

		//getintstring needs to recognize whether you can take halter, lead or both. content id can be halter&lead, and then add all three options (take one, take the other, take both) to the currentlyrelevantids
		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.TAKE_HALTER:
			TakeContent (player, equippableItemID.HALTER);
			break;
		case actionID.TAKE_LEAD:
			TakeContent (player, equippableItemID.LEAD);
			break;
		case actionID.TAKE_HALTER_AND_LEAD:
			TakeContent (player, equippableItemID.HALTER_WITH_LEAD);
			break;
		case actionID.HANG_UP_HALTER:

			break;
		case actionID.HANG_UP_LEAD:

			break;
		case actionID.HANG_UP_HALTER_AND_LEAD:

			break;
		}

	}

	private void TakeContent(Player player, equippableItemID contentID){
	
	}

	private void HangUpGear(Player player){
	//content = player.currentlyequipped
	}

	public override List<string> DefineInteraction (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear();

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS: 
			if (hookStatus == containerStatus.FULL) {
				if (content.id == equippableItemID.HALTER) {
					currentlyRelevantActionIDs.Add (actionID.TAKE_HALTER);
					result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_HALTER));
				} else if (content.id == equippableItemID.LEAD) {
					currentlyRelevantActionIDs.Add (actionID.TAKE_LEAD);
					result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_LEAD));
				} else if (content.id == equippableItemID.HALTER_WITH_LEAD) {
					currentlyRelevantActionIDs.Add (actionID.TAKE_HALTER_AND_LEAD);
					result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_HALTER_AND_LEAD));
				}
			}
			break;
		case equippableItemID.HALTER:
			if (hookStatus == containerStatus.EMPTY) {
				currentlyRelevantActionIDs.Add(actionID.HANG_UP_HALTER);
				result.Add(InteractionStrings.GetInteractionStringById(actionID.HANG_UP_HALTER));
			}
			break;
		case equippableItemID.LEAD:
			if (hookStatus == containerStatus.EMPTY) {
				currentlyRelevantActionIDs.Add(actionID.HANG_UP_LEAD);
				result.Add(InteractionStrings.GetInteractionStringById(actionID.HANG_UP_LEAD));
			}
			break;
		case equippableItemID.HALTER_WITH_LEAD:
			if (hookStatus == containerStatus.EMPTY) {
				currentlyRelevantActionIDs.Add(actionID.HANG_UP_HALTER_AND_LEAD);
				result.Add(InteractionStrings.GetInteractionStringById(actionID.HANG_UP_HALTER_AND_LEAD));
			}
			break;
		}

		return result;

	}
}
