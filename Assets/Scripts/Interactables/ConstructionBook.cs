using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionBook : Interactable {

	public int unlockedStalls;

	public StallPaddockUnit[] stalls;

	/*
	 * select upgrade, pay in money
	 * 		choose to add stable space
	 * 		Enable or disable paddock partitions
	 * 		
	 * Wait for x days 
	 * See construction site in the meantime
	 * 
	 * 
	 * in save: gotta add which are under construction and how long they got left? (resp how many days have passed since start construction, or store completion day)
	 * have a list of all buildings under construction with their completion day and check all of those in the morning? 
	 * 
	 * store upgrade prices in Prices doc, get UI value from there and take price from there on starting construction 
	 * 
	 * */

	public void SetUnlockedStallsFromSave(int unlocked){
		unlockedStalls = unlocked;

		//Apply, make visible
		for (int i = 0; i < unlocked; ++i){
			stalls [i].gameObject.SetActive (true);
		}
	}

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.OPEN_MENU:
			OpenConstructionWindow ();
			break;
		}
	}

	public override List<string> DefineInteraction (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear();

		result.Add (InteractionStrings.GetInteractionStringById (actionID.OPEN_MENU));
		currentlyRelevantActionIDs.Add (actionID.OPEN_MENU);

		return result;

	}

	private void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			CloseWindow ();
		}
	}

	public void StartBuilding(string buttonContent){


		if (buttonContent == "add-stall") {
			//start construction
			//stalls[unlockedStalls]
		}

	}

	private void OpenConstructionWindow(){
		UI.instance.constructionUI.SetActive (true);
	}

	public void CloseWindow(){
		UI.instance.constructionUI.SetActive (false);
	}
}
