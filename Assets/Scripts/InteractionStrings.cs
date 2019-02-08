using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionStrings : MonoBehaviour {

	public static Dictionary<actionID, string> allInteractionStrings = new Dictionary<actionID, string>();

	public static void SetupStrings(){
		allInteractionStrings.Add (actionID._EMPTYSTRING, "");
		allInteractionStrings.Add (actionID.OPEN_CLOSE_DOOR, "Open / Close");
		allInteractionStrings.Add (actionID.PICK_UP, "Pick Up");
		allInteractionStrings.Add (actionID.BRUSH_HORSE, "Brush Horse");
		allInteractionStrings.Add (actionID.CLEAN_MANURE, "Clean Manure");
		allInteractionStrings.Add (actionID.EMPTY_PITCHFORK, "Empty Pitchfork");
		allInteractionStrings.Add (actionID.EMPTY_WHEELBARROW, "Empty Wheelbarrow");
		allInteractionStrings.Add (actionID.PUSH_WHEELBARROW, "Push Wheelbarrow");
		allInteractionStrings.Add (actionID.FEED_HORSE, "Feed Horse");
		allInteractionStrings.Add (actionID.FILL_BUCKET, "Fill Bucket");
		allInteractionStrings.Add (actionID.NO_SEQUENCE, "");
		allInteractionStrings.Add (actionID.PET_HORSE, "Pet Horse");
		allInteractionStrings.Add (actionID.WATER_HORSE, "Water Horse");
		allInteractionStrings.Add (actionID.TAKE_STRAW, "Take Portion");
		allInteractionStrings.Add (actionID.PUT_AWAY_STRAW, "Return Portion");
		allInteractionStrings.Add (actionID.TAKE_HALTER, "Take Halter");
		allInteractionStrings.Add (actionID.TAKE_LEAD, "Take Lead");
		allInteractionStrings.Add (actionID.TAKE_HALTER_AND_LEAD, "Take Halter and Lead");
		allInteractionStrings.Add (actionID.HANG_UP_HALTER, "Hang Up Halter");
		allInteractionStrings.Add (actionID.HANG_UP_LEAD, "Hang Up Lead");
		allInteractionStrings.Add (actionID.HANG_UP_HALTER_AND_LEAD, "Hang Up Halter and Lead");
		allInteractionStrings.Add (actionID.PUT_ON_HALTER, "Put on Halter");
		allInteractionStrings.Add (actionID.PUT_ON_LEAD, "Put on Lead");
		allInteractionStrings.Add (actionID.PUT_ON_HALTER_AND_LEAD, "Put on Halter and Lead");
		allInteractionStrings.Add (actionID.LEAD_HORSE, "Lead Horse");
		allInteractionStrings.Add (actionID.TIE_HORSE_TO_POST, "Tie Horse to Post");
		allInteractionStrings.Add (actionID.TAKE_SADDLE_WITH_PAD, "Take Saddle and Pad");
		allInteractionStrings.Add (actionID.PUT_ON_SADDLE_WITH_PAD, "Put on Saddle and Pad");
		allInteractionStrings.Add (actionID.HANG_UP_SADDLE_WITH_PAD, "Stow Saddle and Pad");
		allInteractionStrings.Add (actionID.TAKE_BRIDLE, "Take Bridle");
		allInteractionStrings.Add (actionID.PUT_ON_BRIDLE, "Put on Bridle");
		allInteractionStrings.Add (actionID.HANG_UP_BRIDLE, "Hang up Bridle");
		allInteractionStrings.Add (actionID.LEAD_BY_REINS, "Lead Horse by Reins");
		allInteractionStrings.Add (actionID.STOP_LEADING_BY_REINS, "Stop Leading by Reins");
		allInteractionStrings.Add (actionID.MOUNT_HORSE, "Mount Horse");
		allInteractionStrings.Add (actionID.TALK_TO, "Talk");
		allInteractionStrings.Add (actionID.SELL, "Sell: ");
		allInteractionStrings.Add (actionID.RESTOCK_CART, "Restock");
		allInteractionStrings.Add (actionID.REFILL_CART, "Refill Cart");
		allInteractionStrings.Add (actionID.PUT_INTO_POCKET, "Put Away (Pocket)");



		allInteractionStrings.Add (actionID.DISMOUNT_HORSE, "Dismount"); //this is probably not used anywhere
	}

	public static string GetInteractionStringById(actionID id){
		if (allInteractionStrings.Count == 0) {
			SetupStrings ();
		}

		string result = "";

		allInteractionStrings.TryGetValue (id, out result);
		if (result == "") {
			result = id.ToString ();
			Debug.LogWarning ("no interaction string for " + result);
		}

		return result;
	}
}
