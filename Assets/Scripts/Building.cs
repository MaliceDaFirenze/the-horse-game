using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

	public GameObject hideOnEntry;
	private bool playerIsInside;
	
	public void PlayerEntersEntranceTrigger(){
		if (playerIsInside) {
			playerIsInside = false;
			hideOnEntry.SetActive (true);
		} else {
			playerIsInside = true;
			hideOnEntry.SetActive (false);
		}
	}
}
