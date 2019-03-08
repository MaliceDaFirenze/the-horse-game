using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionBook : Interactable {


	/*
	 * select upgrade, pay in money
	 * 		choose to add stable space
	 * 		Enable or disable paddock partitions
	 * 		
	 * Wait for x days 
	 * See construction site in the meantime
	 * 
	 * 
	 * 
	 * */

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
	
	}

	private void OpenConstructionWindow(){
		UI.instance.constructionUI.SetActive (true);
	}

	public void CloseWindow(){
		UI.instance.constructionUI.SetActive (false);
	}
}
