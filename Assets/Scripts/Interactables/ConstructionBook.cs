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
	 * */

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.RESTOCK_CART:

			break;
		}
	}


	public override List<string> DefineInteraction (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear();




		return result;

	}
}
