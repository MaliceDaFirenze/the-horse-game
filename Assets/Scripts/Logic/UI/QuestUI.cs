using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour {

	//have containers for Quest UI
	//when switching tabs to open, active, done & all quests, refresh the contents
	//display: icon 
		//active/done etc. three dots or a tick. 
		//questgiver? maybe later
		// quest name
		// current objective
		//done objectives?

	//display
	public Transform containerParent; //spawn new containers as child of this
	public List<QuestUIContainer> questContainers = new List<QuestUIContainer>(); //either pool these or just create/destroy for now? 

	public void ShowQuestUI(bool show){
		gameObject.SetActive (show);
		Quests.instance.questUIVisible = show;
	}

	public void ChangeTab(string id){
		//active here actually includes open and active
	}

	public void DisplayQuest (Quest quest){
		QuestUIContainer newQuestUIContainer = Instantiate (PrefabManager.instance.questUIContainer, containerParent).GetComponent<QuestUIContainer>();
		questContainers.Add (newQuestUIContainer);

		newQuestUIContainer.titleTextBox.text = quest.name;
		newQuestUIContainer.mainTextBox.text = quest.instructions[0][quest.progressIndex];

	}
}
