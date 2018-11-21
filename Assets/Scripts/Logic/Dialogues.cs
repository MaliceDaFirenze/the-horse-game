using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character {
	GRANDMA,
	MAYOR,
	STORECLERK
}

public enum DialogueID{
	GREETING,
	SALE
}



public struct dialogueLine {
	public int day;
	public Character character;
	public DialogueID id;

	public dialogueLine(int _day, Character _character, DialogueID _id){
		day = _day;
		character = _character;
		id = _id;
	}
}

public class Dialogues {
	
	private static bool initialSetupComplete;
	private static Dictionary<dialogueLine, string> allDialogues = new Dictionary<dialogueLine, string> ();

	public static string RetrieveDialogue(int day, Character character, DialogueID id){
		if (!initialSetupComplete){
			Setup ();
		}

		//Can I check if there's a line for the day, char and id Im looking for by making a new struct? 

		string result = "notFound";

		allDialogues.TryGetValue (new dialogueLine (day, character, id), out result);

		Debug.Log ("dialogue returns: " + result);
		return result;
	}

	private static void Setup(){

		allDialogues.Add (new dialogueLine(0, Character.GRANDMA, DialogueID.GREETING), "It's good to have you here, dear. Here's some cash to buy supplies for the horse");
		allDialogues.Add (new dialogueLine(0, Character.STORECLERK, DialogueID.GREETING), "Oh, you're the new person! What can I do for you?");
		allDialogues.Add (new dialogueLine(1, Character.STORECLERK, DialogueID.GREETING), "Hi, welcome back! What can I do for you?");


		initialSetupComplete = true;
	}
}
