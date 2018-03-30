using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	public GameObject instructionGO;
	public Text instructionText;

	void Start(){
		HideInstruction ();
	}

	public void ShowInstruction(Interactable interactable){
		instructionGO.SetActive (true);
		instructionText.text = "E - " + interactable.action;
	}

	public void HideInstruction(){
		instructionGO.SetActive (false);
	}
}
