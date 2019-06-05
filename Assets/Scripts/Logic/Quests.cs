using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestID {
	FIRST_FEEDING,
	FIRST_SUPPLIES
}

public enum QuestTask{
	TALK_TO_GRANDMA,
	TAKE_HAY,
	DROP_HAY_IN_PADDOCK
}

public enum QuestStatus {
	UNAVAILABLE,	//quest not yet available
	OPEN,			//quest available, but nothing done yet
	ACTIVE,			//some tasks done, some still open
	COMPLETED 		//all tasks done
}

public struct Quest {
	public QuestID id;
	public string name;
	public int progressIndex;
	public QuestStatus status;
	public List<Dictionary<QuestTask, bool>> conditions; //every step of the quest can have one or several bools that need to be true in order to progress
	public List<List<string>> instructions; //instructions for every step of the quest
}

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
	 * 
	 * 
	 * store quests in a dictionary with the key being a struct of conditions to unlock the quest? 
	 * just use list for now, figure out the rest later
	 * */

	public bool questUIVisible;
	private QuestUI _questUI;
	public QuestUI questUI { 
		get { 
			if (_questUI == null) {
				_questUI = FindObjectOfType<QuestUI> ();
			} 
			return _questUI;
		} 
	}

	private bool questsInitialized;
	private Dictionary<QuestID, Quest> allQuests = new Dictionary<QuestID, Quest>();
	private Dictionary<QuestID, Quest> activeQuests = new Dictionary<QuestID, Quest>();

	private void Start(){
		questUIVisible = false;
		questUI.gameObject.SetActive (false);
	}

	public void ActivateQuest(QuestID id){
		//activate first quest from timelogic/new day
		//display goals in quest UI
		//on interacting, check if it was achieved? 
		questUI.DisplayQuest(GetQuest(id));
		activeQuests.Add (id, GetQuest(id));
	}

	private Quest GetQuest(QuestID id){
		if (!questsInitialized) {
			InitQuests ();
		}

		return allQuests [id];
	}

	public void FulfilledQuestCondition(QuestTask task){
		foreach (KeyValuePair<QuestID, Quest> activeQuest in activeQuests) {
			if (activeQuest.Value.conditions [activeQuest.Value.progressIndex].ContainsKey (task)) {
				activeQuest.Value.conditions [activeQuest.Value.progressIndex] [task] = true;
			}			
		}
	}

	private void InitQuests(){
	
		Quest newQuest = new Quest ();

		newQuest.id = QuestID.FIRST_FEEDING;
		newQuest.status = QuestStatus.OPEN;
		newQuest.name = "Food for the horse";
		newQuest.progressIndex = 0;
		newQuest.conditions = new List<Dictionary<QuestTask, bool>> ();
		newQuest.instructions = new List<List<string>> ();

		newQuest.conditions.Add(new Dictionary<QuestTask, bool>{
			{ QuestTask.TALK_TO_GRANDMA, false},
			{ QuestTask.TAKE_HAY , false},
			{ QuestTask.DROP_HAY_IN_PADDOCK, false}
		});

		newQuest.instructions.Add(new List<string>{
			"Talk to Grandma",
			"Take a portion of hay from haystack",
			"Drop the hay in the paddock"
		});

		//wait why are instructions a list of lists, and not just a list? Why so convoluted? 

		allQuests.Add (newQuest.id, newQuest);

		questsInitialized = true;
	}

	//---Singleton---//
	private static Quests _instance;
	public static Quests instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<Quests> ();
				if (_instance == null) {
					GameObject go = new GameObject ();
					go.name = "QuestsGO";
					_instance = go.AddComponent<Quests> ();
				}
				DontDestroyOnLoad (_instance.gameObject);
			} 
			return _instance;
		}
	}
}

