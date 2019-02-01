using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum storeCaseType{
	BUY_CASE,
	RESTOCK_CASE
}

public class StoreCase : Interactable {

	public HayCart cartToRestock;
	public storeCaseType storeType;

	private int restockPrice = 10;
	private int refillPrice = 80;

	private int restockUnits = 10;
	private int refillUnits = 100;


	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.RESTOCK_CART:

			if (PlayerEconomy.Money >= restockPrice) {
				RestockHayCart ();
			} else {
				Debug.Log ("not enough money to restock!");
			}
			break;
		case actionID.REFILL_CART:
			if (PlayerEconomy.Money >= refillPrice) {
				RefillHayCart ();
			} else {
				Debug.Log ("not enough money to refill!");
			}	
			break;
		}
	}

	private void RestockHayCart(){
		PlayerEconomy.PayMoney (restockPrice);
		cartToRestock.InitOrRestockCart (cartToRestock.currentUnits + restockUnits);
	}

	private void RefillHayCart(){
		PlayerEconomy.PayMoney (refillPrice);
		cartToRestock.InitOrRestockCart (cartToRestock.currentUnits + refillUnits);
	}

	public override List<string> DefineInteraction (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear();


		switch (storeType) {
		case storeCaseType.RESTOCK_CASE:

			switch (player.currentlyEquippedItem.id) {
			case equippableItemID.BAREHANDS:
				//NEXT: add different options, like restock 10 for x credits, or fill cart for y credits

				currentlyRelevantActionIDs.Add (actionID.RESTOCK_CART);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.RESTOCK_CART) + "(" + cartToRestock.fillType + "x" + restockUnits +") - ¢ "+ restockPrice);

				currentlyRelevantActionIDs.Add (actionID.REFILL_CART);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.REFILL_CART) + "(" + cartToRestock.fillType + "x" + refillUnits+") - ¢ "+ refillPrice);
				break;
			}
			break;
		case storeCaseType.BUY_CASE:
			//todo 
			break;
		}

		return result;

	}
}
