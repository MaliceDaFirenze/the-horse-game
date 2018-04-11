using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	public GameObject instructionGO;
	public Text instructionText;

	public GameObject arrowSequenceGO;
	public Image[] arrows;

	public HorseUI horseUI;

	void Start(){
		HideInstruction ();
	}

	public void ShowInstruction(Interactable interactable, Player player){
		if (interactable.GetInteractionString (player) != "") {
			instructionGO.SetActive (true);
			instructionText.text = "E - " + interactable.GetInteractionString(player);

			dir[] sequence = ArrowSequences.GetArrowSequence (interactable.currentlyRelevantActionID);
			if (sequence != null) {
				for (int i = 0; i < arrows.Length; ++i) {
					if (i < sequence.Length) {
						arrows[i].rectTransform.eulerAngles = new Vector3(0,0, 90 * (int)sequence[i]); 
						arrows [i].enabled = true;
					} else {
						arrows [i].enabled = false;
					}
				}
				arrowSequenceGO.SetActive (true);
			}
				
		}
	}

	public void HideInstruction(){
		instructionGO.SetActive (false);
		arrowSequenceGO.SetActive (false);
	}

	public void ShowHorseUI (Horse_Stats horse){
		horseUI.ShowUIForHorse (horse);
	}

	public void HideHorseUI(){
		horseUI.Hide ();
	}
}
