using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroLogic : MonoBehaviour {

	public Sprite grandmaPortrait;
	public Sprite playerPortrait;

	private int introSteps = 8;
	private int currentIntroIndex = 0;
	private int dialogueIndex = 0;

	void Update(){
		if (Input.GetMouseButtonDown (0)) {

			if (currentIntroIndex >= introSteps) {
				SceneManager.LoadScene ("main");
			} else {
				if (currentIntroIndex % 2 == 0) {
					UI.instance.ShowDialogue (Dialogues.RetrieveDialogue (0, Character.GRANDMA, DialogueID.INTRO)[dialogueIndex], grandmaPortrait, Character.GRANDMA);
					//GameObject.Find ("DebugText").GetComponent<Text> ().text = "ui instance: " + UI.instance.name + ". dialogueText is active: " + UI.instance.dialogueText.gameObject.activeSelf + ". this pos: " + GameObject.Find ("DebugText").transform.position + ". dialogue box pos: " + UI.instance.dialogueText.transform.position;
				} else {
					UI.instance.ShowDialogue (Dialogues.RetrieveDialogue (0, Character.PLAYER, DialogueID.INTRO)[dialogueIndex], playerPortrait, Character.PLAYER);
					//GameObject.Find ("DebugText").GetComponent<Text> ().text = Dialogues.RetrieveDialogue (0, Character.PLAYER, DialogueID.INTRO) [dialogueIndex];
				}

				++currentIntroIndex;
				dialogueIndex = currentIntroIndex / 2;
			}
		}
	}
}
