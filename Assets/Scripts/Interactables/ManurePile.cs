using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManurePile : Interactable {

	private Transform[] balls; 

	private float minWait = 0.5f;
	private float maxWait = 1.5f;

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

	}

	public IEnumerator GetProduced(){
		balls = GetComponentsInChildren<Transform>(true);
		yield return new WaitForSeconds (minWait);

		for (int i = 0; i < balls.Length; ++i) {
			balls [i].gameObject.SetActive (true);

			if (Random.value < 0.5f) {
				yield return new WaitForSeconds (Random.Range (minWait, maxWait));
			}
		} 
	}

	public override string GetInteractionString (Player player)	{
		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS: 
			return emptyHandsAction;
		case equippableItemID.PITCHFORK:
			currentlyRelevantActionID = actionID.CLEAN_MANURE;
			return "Clean Manure";
		default: 
			return "";
		}
	}
}
