using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseGear_Interactable : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.TAKE_HALTER:
			PickUpHorseGear (player, equippableItemID.HALTER);
			break;
		case actionID.TAKE_LEAD:
			PickUpHorseGear (player, equippableItemID.LEAD);
			break;
		case actionID.TAKE_HALTER_AND_LEAD:
			PickUpHorseGear (player, equippableItemID.HALTER_WITH_LEAD);
			break;
		}

	}

	private void PickUpHorseGear(Player player, equippableItemID itemToTake){
		Equippable combined = null;
		Equippable halter = null;
		Equippable lead = null;

		if (equippable.id == equippableItemID.HALTER_WITH_LEAD) {
			combined = equippable;
			foreach (Transform child in equippable.transform) {
				Equippable childEquippable = child.GetComponent<Equippable> ();
				if (childEquippable.id == equippableItemID.HALTER) {
					halter = childEquippable;
				} else if (childEquippable.id == equippableItemID.LEAD) {
					lead = childEquippable;
				}
			}
		}

		switch (itemToTake){
		case equippableItemID.HALTER:
			//if content.id is halter and lead, but i only want to take halter, unparent lead and halter from halter_w_lead. take halter, lead remains
			if (equippable.id == equippableItemID.HALTER_WITH_LEAD) {
				halter.BeEquipped ();
				player.EquipAnItem (halter);
				combined.transform.SetParent (halter.transform);
				lead.transform.SetParent (null);
				lead.GetComponent<SphereCollider> ().enabled = true;
			} else if (equippable.id == equippableItemID.HALTER){
				PickUpAll (player);
			}
			break;
		case equippableItemID.LEAD:
			//if content.id is halter and lead, but i only want to take halter, unparent lead and halter from halter_w_lead. take lead, halter remains
			if (equippable.id == equippableItemID.HALTER_WITH_LEAD) {
				lead.BeEquipped ();
				player.EquipAnItem (lead);
				halter.transform.SetParent (null);
				combined.transform.SetParent (halter.transform);
				halter.GetComponent<SphereCollider> ().enabled = true;
				combined.GetComponent<SphereCollider> ().enabled = false;
			} else if (equippable.id == equippableItemID.LEAD){
				PickUpAll (player);
			}
			break;
		case equippableItemID.HALTER_WITH_LEAD:
			PickUpAll (player);
			break;
		}
	}

	private void PickUpAll(Player player){
		player.EquipAnItem(equippable);
		equippable.BeEquipped ();
	}

	public override List<string> DefineInteraction (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear();

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS: 
			if (equippable.id == equippableItemID.HALTER) {
				currentlyRelevantActionIDs.Add (actionID.TAKE_HALTER);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_HALTER));
			} else if (equippable.id == equippableItemID.LEAD) {
				currentlyRelevantActionIDs.Add (actionID.TAKE_LEAD);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_LEAD));
			} else if (equippable.id == equippableItemID.HALTER_WITH_LEAD) {
				//if halter and lead are hanging there, you can take one, the other or both
				currentlyRelevantActionIDs.Add (actionID.TAKE_HALTER_AND_LEAD);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_HALTER_AND_LEAD));
				currentlyRelevantActionIDs.Add (actionID.TAKE_HALTER);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_HALTER));
				currentlyRelevantActionIDs.Add (actionID.TAKE_LEAD);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_LEAD));
			}
			break;
		}
		return result;
	}
}