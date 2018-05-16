using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionStrings : MonoBehaviour {

	public static Dictionary<actionID, string> allInteractionStrings = new Dictionary<actionID, string>();

	public static void SetupStrings(){
		allInteractionStrings.Add (actionID._EMPTYSTRING, "");
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
		allInteractionStrings.Add (actionID.PUT_AWAY_STRAW, "Put Straw Away");
		allInteractionStrings.Add (actionID.TAKE_HALTER, "Take Halter");
		allInteractionStrings.Add (actionID.TAKE_LEAD, "Take Lead");
		allInteractionStrings.Add (actionID.HANG_UP_HALTER, "Hang Up Halter");
		allInteractionStrings.Add (actionID.HANG_UP_LEAD, "Hang Up Lead");
		allInteractionStrings.Add (actionID.PUT_ON_HALTER, "Put on Halter");
		allInteractionStrings.Add (actionID.PUT_ON_LEAD, "Put on Lead");
		allInteractionStrings.Add (actionID.PICK_UP, "Pick Up");
	}

	public static string GetInteractionStringById(actionID id){
		if (allInteractionStrings.Count == 0) {
			SetupStrings ();
		}

		return allInteractionStrings [id];
	}
}
