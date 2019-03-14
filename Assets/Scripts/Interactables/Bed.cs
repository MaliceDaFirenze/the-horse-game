using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		FindObjectOfType<TimeLogic> ().EndDay ();
	}

	public override List<string> DefineInteraction(Player player){
		currentlyRelevantActionIDs.Clear ();
		List<string> result = new List<string> ();

		currentlyRelevantActionIDs.Add (actionID.SLEEP);
		result.Add (InteractionStrings.GetInteractionStringById(actionID.SLEEP));

		return result;
	}
}
