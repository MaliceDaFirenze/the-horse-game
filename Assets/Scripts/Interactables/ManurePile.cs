﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManurePile : Interactable {

	private Transform[] balls; 

	private float minWait = 0.5f;
	private float maxWait = 1.5f;

	void Start(){
		StartCoroutine (GetProduced ());
	}

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);
		//how does filling pitchfork work?
		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.PITCHFORK:
			if (player.currentlyEquippedItem.status == containerStatus.EMPTY) {
				player.currentlyEquippedItem.status = containerStatus.FULL;
				transform.SetParent (player.currentlyEquippedItem.transform);
				transform.position = player.currentlyEquippedItem.fillNullPos.position;
				EnableAllColliders (false);
				player.currentlyEquippedItem.content = gameObject;
			}
			break;
		default:
			break;
		}
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

	public void EnableAllColliders (bool enable){
		Collider[] allColliders = GetComponentsInChildren<Collider> ();
		for (int i = 0; i < allColliders.Length; ++i) {
			allColliders [i].enabled = enable;
		}
	}

	public override List<string> GetInteractionStrings (Player player)	{

		List<string> result = new List<string> ();
		currentlyRelevantActionID = actionID._EMPTYSTRING;

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.PITCHFORK:
			if (player.currentlyEquippedItem.status == containerStatus.EMPTY) {
				currentlyRelevantActionID = actionID.CLEAN_MANURE;
			}
			break;
		}

		result.Add(InteractionStrings.GetInteractionStringById(currentlyRelevantActionID));
		return result;

	}
}
