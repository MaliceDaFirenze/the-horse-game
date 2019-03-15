using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionBook : Interactable {

	public int unlockedStalls;

	public Dictionary<int, int> constructionDaysRemainingPerStallIndex = new  Dictionary<int, int> ();

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
	 * 
	 * */

	public void SetUnlockedStallsFromSave(int unlocked, Dictionary<int, int> underConstruction){
		unlockedStalls = unlocked;

		//Apply, make visible
		for (int i = 0; i < unlocked; ++i){
			stalls [i].gameObject.SetActive (true);
		}

		constructionDaysRemainingPerStallIndex = underConstruction;
	}

	public void NewDay(){

		Dictionary<int, int> newDict = new  Dictionary<int, int> ();
		foreach (KeyValuePair<int, int> info in constructionDaysRemainingPerStallIndex) {

			if (info.Value == 1) {

				stalls [info.Key].underConstruction.SetActive (false);
				for (int i = 0; i < stalls [info.Key].finished.Length; ++i) {
					stalls [info.Key].finished [i].SetActive (true);
				}
			} else {
				newDict.Add(info.Key, info.Value - 1);
			}
		} 

		constructionDaysRemainingPerStallIndex = newDict;
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

		Debug.Log ("button click: " + buttonContent);
		if (buttonContent == "add-stall") {

			if (PlayerEconomy.Money >= Prices.GetStallConstructionPrice (unlockedStalls)) {
				PlayerEconomy.PayMoney (Prices.GetStallConstructionPrice (unlockedStalls));


				//enable GO, construction
				stalls [unlockedStalls].gameObject.SetActive (true);
				stalls [unlockedStalls].underConstruction.SetActive (true);
				for (int i = 0; i < stalls [unlockedStalls].finished.Length; ++i) {
					stalls [unlockedStalls].finished [i].SetActive (false);
				}

				//store in duration dict. index is nr of unlocked stalls
				constructionDaysRemainingPerStallIndex.Add(unlockedStalls, Durations.GetStallConstructionDuration (unlockedStalls));


				unlockedStalls++;

			} else {
				Debug.Log ("not enough money for stall construction!");
			}
			//start construction
			//stalls[unlockedStalls]
		}

	}

	private void OpenConstructionWindow(){
		UI.instance.constructionUI.gameObject.SetActive (true);

		//the number of unlocked stalls (i.e. 1 at the start) equals the index of the next to unlock
		UI.instance.constructionUI.priceForNextStall.text = Prices.GetStallConstructionPrice (unlockedStalls).ToString() + "¢"; 
		UI.instance.constructionUI.durationForNextStall.text = Durations.GetStallConstructionDuration (unlockedStalls).ToString() + " Days";
	}

	public void CloseWindow(){
		UI.instance.constructionUI.gameObject.SetActive (false);
	}
}
