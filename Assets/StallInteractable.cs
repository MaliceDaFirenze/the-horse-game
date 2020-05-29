using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StallInteractable : Interactable {

	private StallDirt stallDirt;

	private void Start(){
		stallDirt = GetComponent<StallDirt> ();
	}

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.CLEAN_STALL:
			stallDirt.Clean ();
			break;
		}
	}

	public override List<string> DefineInteraction (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear ();

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.PITCHFORK:
			if (stallDirt.dirtLevel > 0) {
				currentlyRelevantActionIDs.Add (actionID.CLEAN_STALL);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.CLEAN_STALL));
			}
			break;
		}
		return result;
	}
}
