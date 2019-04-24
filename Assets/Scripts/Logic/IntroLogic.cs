using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IntroLogic : MonoBehaviour {

	public Sprite portrait;

	private int introSteps = 4;
	private int currentIntroIndex = 0;

	// Use this for initialization
	void Start () {
		UI.instance.ShowDialogue (Dialogues.RetrieveDialogue (0, Character.GRANDMA, DialogueID.INTRO)[0], portrait, Character.GRANDMA);

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
