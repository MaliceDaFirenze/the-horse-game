using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prices : MonoBehaviour {

	public static Dictionary<equippableItemID, int> allSellingPrices = new Dictionary<equippableItemID, int>();
	public static List<int> stallConstructionPrices = new List<int>();

	public static void Setup(){
		allSellingPrices.Add (equippableItemID.BLUEGEM, 20); 
		allSellingPrices.Add (equippableItemID.REDGEM, 75); 
		allSellingPrices.Add (equippableItemID.GREENGEM, 250); 
		allSellingPrices.Add (equippableItemID.SMALLANTLER, 6); 
		allSellingPrices.Add (equippableItemID.BIGANTLER, 10); 
		allSellingPrices.Add (equippableItemID.SMALLBRANCH, 1); 
		allSellingPrices.Add (equippableItemID.BIGBRANCH, 5); 
		allSellingPrices.Add (equippableItemID.APPLE, 3); 

		stallConstructionPrices.Add (0);
		stallConstructionPrices.Add (700);
		stallConstructionPrices.Add (1500);
		stallConstructionPrices.Add (3000);
	}

	public static int GetPriceByID(equippableItemID id){
		if (allSellingPrices.Count == 0) {
			Setup ();
		}

		int result = -1;

		allSellingPrices.TryGetValue (id, out result);
		if (result == -1) {
			Debug.LogWarning ("no price for " + result);
		}

		return result;
	}

	public static int GetStallConstructionPrice(int stallIndex){
		if (stallConstructionPrices.Count == 0) {
			Setup ();
		}

		int result = -1;

		if (stallIndex < stallConstructionPrices.Count) {
			result = stallConstructionPrices [stallIndex];
		} else {
			Debug.LogWarning ("no construction price for stall index " + stallIndex);
		}

		return result;
	}
}
