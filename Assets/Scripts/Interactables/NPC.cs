﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable {

	public Character characterId;
	public Sprite portrait;

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		if (currentlyRelevantActionIDs.Count > selectedInteractionIndex) {
			switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
			case actionID.TALK_TO:
				

				UI.instance.ShowDialogue (Dialogues.RetrieveDialogue (TimeLogic.day, characterId, DialogueID.GREETING), portrait, characterId);
				//open dialogue window
				break;
			}
		}
	}

	public override List<string> DefineInteraction(Player player){
		currentlyRelevantActionIDs.Clear ();
		List<string> result = new List<string> ();

		result.Add (InteractionStrings.GetInteractionStringById(actionID.TALK_TO));
		currentlyRelevantActionIDs.Add (actionID.TALK_TO);

		return result;
	}
}
