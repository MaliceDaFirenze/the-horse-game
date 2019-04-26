using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
				} else {
					UI.instance.ShowDialogue (Dialogues.RetrieveDialogue (0, Character.PLAYER, DialogueID.INTRO)[dialogueIndex], playerPortrait, Character.PLAYER);
				}

				++currentIntroIndex;
				dialogueIndex = currentIntroIndex / 2;
			}
		}
	}
}
