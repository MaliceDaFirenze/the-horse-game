using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionBook : Interactable {

	public int unlockedStalls;

	public Dictionary<int, int> constructionDaysRemainingPerStallIndex = new  Dictionary<int, int> ();

	public StallPaddockUnit[] stalls;

	/*
	 * 
	 * save and restore paddock wall status
	 * 
	 * */

	public void SetUnlockedStallsFromSave(int unlocked, Dictionary<int, int> underConstruction){
		unlockedStalls = unlocked;

		//Apply, make visible
		for (int i = 0; i < unlocked; ++i){
			stalls [i].gameObject.SetActive (true);
		}

		constructionDaysRemainingPerStallIndex = underConstruction;

		Debug.LogWarning ("set unlocked stalls from save. construction dict count: " + constructionDaysRemainingPerStallIndex.Count);
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
				UI.instance.constructionUI.underConstructionDaysRemaining.text = constructionDaysRemainingPerStallIndex[unlockedStalls].ToString () + " " + GetDayPluralSingular(constructionDaysRemainingPerStallIndex[unlockedStalls]) + " remaining";

				unlockedStalls++;

				UI.instance.constructionUI.hideOnConstruction.SetActive (false);
				UI.instance.constructionUI.showOnConstruction.SetActive (true);


			} else {
				Debug.Log ("not enough money for stall construction!");
			}
		}
	}

	public void TogglePaddockWall(int index, bool enabled){
		Debug.Log ("set toggle " + index + " to " + enabled);

		stalls [index].leftWall.SetActive (enabled);
		stalls [index+1].rightWall.SetActive (enabled);
	}

	private void OpenConstructionWindow(){
		UI.instance.constructionUI.gameObject.SetActive (true);


		if (constructionDaysRemainingPerStallIndex.Count > 0) {
			UI.instance.constructionUI.showOnConstruction.SetActive (true);
			UI.instance.constructionUI.hideOnConstruction.SetActive (false);
			UI.instance.constructionUI.underConstructionDaysRemaining.text = constructionDaysRemainingPerStallIndex[unlockedStalls-1].ToString () + " " + GetDayPluralSingular(constructionDaysRemainingPerStallIndex[unlockedStalls-1]) + " remaining";
		} else {
			UI.instance.constructionUI.showOnConstruction.SetActive (false);
			UI.instance.constructionUI.hideOnConstruction.SetActive (true);
		}



		//the number of unlocked stalls (i.e. 1 at the start) equals the index of the next to unlock
		UI.instance.constructionUI.priceForNextStall.text = Prices.GetStallConstructionPrice (unlockedStalls).ToString() + "¢"; 
		UI.instance.constructionUI.durationForNextStall.text = Durations.GetStallConstructionDuration (unlockedStalls).ToString() + " " + GetDayPluralSingular(Durations.GetStallConstructionDuration (unlockedStalls));

		//partition text
		if (unlockedStalls == 1) {
			UI.instance.constructionUI.partitionText.text = "You only have one stall, all walls are needed.";
		} else {
			UI.instance.constructionUI.partitionText.text = "Click the white toggles to enable or disable the partitions between paddocks.";
		}

		for (int i = 0; i < UI.instance.constructionUI.paddockToggles.Length; ++i) {
			if (i < unlockedStalls) {
				UI.instance.constructionUI.paddockToggles [i].gameObject.SetActive (true);
			} else {
				UI.instance.constructionUI.paddockToggles [i].gameObject.SetActive (false);
			}
			//enable all here so I can disable the right one below, otherwise it won't work again after changes
			UI.instance.constructionUI.paddockToggles [i].toggle.gameObject.SetActive (true);

		}

		UI.instance.constructionUI.paddockToggles [unlockedStalls - 1].toggle.gameObject.SetActive (false);

	}

	public void CloseWindow(){
		UI.instance.constructionUI.gameObject.SetActive (false);
	}

	public static string GetDayPluralSingular(int value){
		if (value == 1) {
			return "Day";
		} else {
			return "Days";
		}

	}
}
