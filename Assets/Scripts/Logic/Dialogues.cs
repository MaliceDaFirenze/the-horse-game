﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character {
	GRANDMA,
	PLAYER,
	MAYOR,
	STORECLERK
}

public enum DialogueID{
	INTRO,
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

	/*multi-line dialogues:
	 * 
	 * 
	 * use string[] instead of string as value in dictionary
	 * create string[] with one or several elements in setup dialogues
	 * when calling showdialogue, pass index for string array? 
	 * 
	 * see ContinueInDialogue in UI script? 
	 * 
	 * how to handle exchanges tho? where one whole dialogue consists of two characters saying things 
	 * 
	 * options: manually call for one after the other 
	 * somehow incorporate all involved characters into the dialogue line struct? 
	 * include empty entries in dialogues for where another characters says something so I can at least increase the index? 
	 * */

	
	private static bool initialSetupComplete;
	private static Dictionary<dialogueLine, string[]> allDialogues = new Dictionary<dialogueLine, string[]> ();
	private static Dictionary<dialogueLine, Reward> allRewards = new Dictionary<dialogueLine, Reward> ();

	public static string[] RetrieveDialogue(int day, Character character, DialogueID id){
		if (!initialSetupComplete){
			Setup ();
		}

		string[] result = new string[] {"notFound"};

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

		//Intro
		allDialogues.Add (new dialogueLine(0, Character.GRANDMA, DialogueID.INTRO), new string[]{"Belle, my dear, I'm so glad you have come!"});
		allDialogues.Add (new dialogueLine(0, Character.PLAYER, DialogueID.INTRO), new string[]{"Of course, Grandma!"});
		//allDialogues.Add (new dialogueLine(0, Character.GRANDMA, DialogueID.INTRO), "I hope not to impose on you too long... Just until I'm back on my feet...");
	//	allDialogues.Add (new dialogueLine(0, Character.PLAYER, DialogueID.INTRO), "Oh that's alright. I don't have... well, I'm very glad to stay as long as you need me to.");

		//TODO: for one dialgogue id, pass and index for progress in that conversation, for dialgoues with more than one line. doesn't make sense to give them all individual dialogue ids

		//Game
		allDialogues.Add (new dialogueLine(1, Character.GRANDMA, DialogueID.GREETING), new string[]{"It's good to have you here, dear. Here's some cash to buy supplies for the horse"});
		allRewards.Add (new dialogueLine (1, Character.GRANDMA, DialogueID.GREETING), new Reward (RewardType.MONEY, 1000)); 
		allDialogues.Add (new dialogueLine(1, Character.STORECLERK, DialogueID.GREETING), new string[]{"Oh, you're the new person! What can I do for you?"});
		allDialogues.Add (new dialogueLine(2, Character.STORECLERK, DialogueID.GREETING), new string[]{"Hi, welcome back! What can I do for you?"});

		initialSetupComplete = true;
	}
}
