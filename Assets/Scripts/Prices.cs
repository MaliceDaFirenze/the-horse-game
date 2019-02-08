using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prices : MonoBehaviour {

	public static Dictionary<equippableItemID, int> sallSellingPrices = new Dictionary<equippableItemID, int>();

	public static void Setup(){
		sallSellingPrices.Add (equippableItemID.BLUEGEM, 20); 
		sallSellingPrices.Add (equippableItemID.REDGEM, 75); 
		sallSellingPrices.Add (equippableItemID.GREENGEM, 250); 
		sallSellingPrices.Add (equippableItemID.SMALLANTLER, 6); 
		sallSellingPrices.Add (equippableItemID.BIGANTLER, 10); 
		sallSellingPrices.Add (equippableItemID.SMALLBRANCH, 1); 
		sallSellingPrices.Add (equippableItemID.BIGBRANCH, 5); 
		sallSellingPrices.Add (equippableItemID.APPLE, 3); 
	}

	public static int GetPriceByID(equippableItemID id){
		if (sallSellingPrices.Count == 0) {
			Setup ();
		}

		int result = -1;

		sallSellingPrices.TryGetValue (id, out result);
		if (result == -1) {
			Debug.LogWarning ("no price for " + result);
		}

		return result;
	}
}
