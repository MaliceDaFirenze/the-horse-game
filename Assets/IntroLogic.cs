using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IntroLogic : MonoBehaviour {

	public Sprite portrait;

	private int introSteps = 1;
	private int currentIntroIndex = 0;

	// Use this for initialization
	void Start () {
		UI.instance.ShowDialogue (Dialogues.RetrieveDialogue (0, Character.GRANDMA, DialogueID.INTRO), portrait, Character.GRANDMA, Dialogues.RetrieveReward(0, Character.GRANDMA, DialogueID.INTRO));

	}

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			++currentIntroIndex;

			if (currentIntroIndex >= introSteps) {
				SceneManager.LoadScene ("main");
			}
		}
	}
}
