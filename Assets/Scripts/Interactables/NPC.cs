using System.Collections;
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
				UI.instance.ShowDialogue (Dialogues.RetrieveDialogue (TimeLogic.day, characterId, DialogueID.GREETING) [0], portrait, characterId, Dialogues.RetrieveReward (TimeLogic.day, characterId, DialogueID.GREETING));


				if (characterId == Character.GRANDMA) {
					Quests.instance.FulfilledQuestCondition (QuestTask.TALK_TO_GRANDMA);
				}

				break;
			case actionID.SELL:
				Equippable item = player.currentlyEquippedItem;
				PlayerEconomy.ReceiveMoney (Prices.GetPriceByID (player.currentlyEquippedItem.id));
				player.UnequipEquippedItem (false, true);
				Destroy (item.gameObject);
				break;
			}
		}
	}

	public override List<string> DefineInteraction(Player player){
		currentlyRelevantActionIDs.Clear ();
		List<string> result = new List<string> ();

		result.Add (InteractionStrings.GetInteractionStringById(actionID.TALK_TO));
		currentlyRelevantActionIDs.Add (actionID.TALK_TO);

		switch (characterId) {
		case Character.STORECLERK: 
			if (player.currentlyEquippedItem.sellable) {
				result.Add (InteractionStrings.GetInteractionStringById(actionID.SELL) + player.currentlyEquippedItem.name + " (" + Prices.GetPriceByID(player.currentlyEquippedItem.id) + ")");
				currentlyRelevantActionIDs.Add (actionID.SELL);
			}

			break;
		default:
			break;
		}

		return result;
	}
}
