using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quests : MonoBehaviour {

	/* quests / tasks are supposed to guide the player through the mechanics and lead them to discover things. 
	 * 
	 * quests could be given by a character and written in their voice? 
	 * 
	 * Or: quests could be set up in dialogue but then show the exact instructions in a more neutral tone. I think this one makes more sense. 
	 * Meaning: quests don't need a "questgiver", necessarily. They will often end with a task to "talk to character again" though. 
	 * 
	 * what do quests need?
	 * not rewards, those are part of dialogue lines that are triggered when you interact with an NPC while at a certain point in a quest
	 * 
	 * 	- a name so they can show up in a quest log
	 * 	- an identifier (enum) 
	 * 	- a progress indicator within the quest, an index
	 * 	- an exact instruction string for every step of the way
	 * 	- conditions for progress at every step of the way. a struct of bools perhaps? for item needed, character talked to etc? 
	 * 	- probably a status of INVISIBLE, IN PROGRESS, DONE
	 * 
	 * */
}
