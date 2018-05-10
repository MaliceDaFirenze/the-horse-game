using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheelbarrow : Interactable {

	public Transform[] contentPositions;
	private int contentPosIndex = 0;

	private List<GameObject> contents = new List<GameObject> ();

	public void Empty(){
		for (int i = 0; i < contents.Count; ++i) {
			Destroy (contents [i]);
		}
		contents.Clear ();
		contentPosIndex = 0;
	}

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS:
			GetComponent<Equippable> ().BeEquipped ();
			player.EquipAnItem (equippable);
			break;
		case equippableItemID.PITCHFORK:
			if (player.currentlyEquippedItem.status == containerStatus.FULL && contentPosIndex < contentPositions.Length) {
				player.currentlyEquippedItem.content.transform.SetParent (transform, true);
				player.currentlyEquippedItem.content.transform.position = contentPositions[contentPosIndex].position;
				++contentPosIndex;
				player.currentlyEquippedItem.content.GetComponent<Collider> ().enabled = false;
				contents.Add (player.currentlyEquippedItem.content);
				player.currentlyEquippedItem.content = null;
				player.currentlyEquippedItem.status = containerStatus.EMPTY;

				//update wheelbarrow status
				if (contentPosIndex == contentPositions.Length) {
					GetComponent<Equippable> ().status = containerStatus.FULL;
				} else {
					GetComponent<Equippable> ().status = containerStatus.PARTIALFULL;
				}
			}
			break;
		default:
			break;
		}
	}

	public override List<string> GetInteractionStrings (Player player)	{

		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear();

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS: 
			currentlyRelevantActionIDs.Add (actionID.PUSH_WHEELBARROW);
			result.Add(InteractionStrings.GetInteractionStringById(actionID.PUSH_WHEELBARROW));
			break;
		case equippableItemID.PITCHFORK:
			if (player.currentlyEquippedItem.status == containerStatus.FULL) {
				currentlyRelevantActionIDs.Add (actionID.EMPTY_PITCHFORK);
				result.Add(InteractionStrings.GetInteractionStringById(actionID.EMPTY_PITCHFORK));
			} 
			break;
		}

		return result;
	}
}
