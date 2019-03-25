using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character {
	GRANDMA,
	PLAYER,
	MAYOR,
	STORECLERK
}

public enum DialogueID{
	GREETING,
	SALE
}

public enum RewardType{
	NONE,
	MONEY
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

public struct Reward{
	public RewardType rewardType; 
	public int rewardAmount;

	public Reward(RewardType _rewardType = RewardType.MONEY, int _rewardAmount = 0){
		rewardType = _rewardType;
		rewardAmount = _rewardAmount;
	}
}

public class Dialogues {
	
	private static bool initialSetupComplete;
	private static Dictionary<dialogueLine, string> allDialogues = new Dictionary<dialogueLine, string> ();
	private static Dictionary<dialogueLine, Reward> allRewards = new Dictionary<dialogueLine, Reward> ();

	public static string RetrieveDialogue(int day, Character character, DialogueID id){
		if (!initialSetupComplete){
			Setup ();
		}

		string result = "notFound";

		allDialogues.TryGetValue (new dialogueLine (day, character, id), out result);

		return result;
	}

	public static Reward RetrieveReward(int day, Character character, DialogueID id){
		if (!initialSetupComplete){
			Setup ();
		}

		Reward result = new Reward(RewardType.NONE, 0);

		allRewards.TryGetValue (new dialogueLine (day, character, id), out result);

		return result;
	}

	private static void Setup(){

		allDialogues.Add (new dialogueLine(1, Character.GRANDMA, DialogueID.GREETING), "It's good to have you here, dear. Here's some cash to buy supplies for the horse");
		allRewards.Add (new dialogueLine (1, Character.GRANDMA, DialogueID.GREETING), new Reward (RewardType.MONEY, 1000)); 
		allDialogues.Add (new dialogueLine(1, Character.STORECLERK, DialogueID.GREETING), "Oh, you're the new person! What can I do for you?");
		allDialogues.Add (new dialogueLine(2, Character.STORECLERK, DialogueID.GREETING), "Hi, welcome back! What can I do for you?");

		initialSetupComplete = true;
	}
}
