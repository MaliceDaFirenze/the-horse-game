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
		}
	}

	public void HideInstruction(){
		instructionGO.SetActive (false);
	}

	public void ShowHorseUI (Horse_Stats horse){
		horseUI.ShowUIForHorse (horse);
	}

	public void HideHorseUI(){
		horseUI.Hide ();
	}
}
