using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

	public GameObject[] hideOnEntry;
	//private bool playerIsInside;

	private MainCam mainCam;
	
	public void PlayerEntersBuildingTrigger(){
		if (mainCam == null) {
			mainCam = FindObjectOfType<MainCam> ();
		}

		for (int i = 0; i < hideOnEntry.Length; ++i) {
			hideOnEntry[i].SetActive (false);
		}
		mainCam.PlayerEntersBuilding ();
	}

	public void PlayerExitsBuildingTrigger(){
		if (mainCam == null) {
			mainCam = FindObjectOfType<MainCam> ();
		}
		for (int i = 0; i < hideOnEntry.Length; ++i) {
			hideOnEntry [i].SetActive (true);
		}
		mainCam.PlayerExitsBuilding ();
	}
}
