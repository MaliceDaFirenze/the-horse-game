using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum buildingType{
	SINGLESTORY,
	MULTISTORY
}

public class Building : MonoBehaviour {

	public GameObject[] hideOnEntry;
	public GameObject[] hideOnFloorOne;
	//private bool playerIsInside;
	public buildingType type;
	public List<string> currentlyActiveTriggers = new List<string>();

	private MainCam mainCam;
	
	public void PlayerEntersBuildingTrigger(string triggerName){
		if (mainCam == null) {
			mainCam = FindObjectOfType<MainCam> ();
		}


		if (type == buildingType.SINGLESTORY) {
			for (int i = 0; i < hideOnEntry.Length; ++i) {
				hideOnEntry [i].SetActive (false);
			}
			mainCam.PlayerEntersBuilding ();
		}  else if (type == buildingType.MULTISTORY) {
			if (!currentlyActiveTriggers.Contains (triggerName)) {
				currentlyActiveTriggers.Add (triggerName);
			}
			UpdateVisibility ();
		}
	}

	public void PlayerExitsBuildingTrigger(string triggerName){
		if (mainCam == null) {
			mainCam = FindObjectOfType<MainCam> ();
		}

		if (type == buildingType.SINGLESTORY) {
			for (int i = 0; i < hideOnEntry.Length; ++i) {
				hideOnEntry [i].SetActive (true);
			}
			mainCam.PlayerExitsBuilding ();
		} else if (type == buildingType.MULTISTORY) {
			if (currentlyActiveTriggers.Contains (triggerName)) {
				currentlyActiveTriggers.Remove (triggerName);
			}
			UpdateVisibility ();
		}
	}

	private void UpdateVisibility(){
		if (!currentlyActiveTriggers.Contains ("UpperFloorTrigger") && !currentlyActiveTriggers.Contains ("LowerFloorTrigger")) {
			//left building entirely
			for (int i = 0; i < hideOnEntry.Length; ++i) {
				hideOnEntry [i].SetActive (true);
			}
			for (int i = 0; i < hideOnFloorOne.Length; ++i) {
				hideOnFloorOne [i].SetActive (true);
			}
			mainCam.PlayerExitsBuilding ();
		} else if (currentlyActiveTriggers.Contains ("UpperFloorTrigger") && currentlyActiveTriggers.Contains ("LowerFloorTrigger")) {
			//moving from one floor to the next
			//check which direction the player is moving in, i.e. what was visible before? 
		} else if (!currentlyActiveTriggers.Contains ("UpperFloorTrigger") && currentlyActiveTriggers.Contains ("LowerFloorTrigger")) {
			for (int i = 0; i < hideOnFloorOne.Length; ++i) {
				hideOnFloorOne [i].SetActive (true);
			}
			for (int i = 0; i < hideOnEntry.Length; ++i) {
				hideOnEntry [i].SetActive (false);
			}
			mainCam.PlayerEntersBuilding ();
		} else if (currentlyActiveTriggers.Contains ("UpperFloorTrigger") && !currentlyActiveTriggers.Contains ("LowerFloorTrigger")) {
			for (int i = 0; i < hideOnEntry.Length; ++i) {
				hideOnEntry [i].SetActive (true);
			}
			for (int i = 0; i < hideOnFloorOne.Length; ++i) {
				hideOnFloorOne [i].SetActive (false);
			}
			mainCam.PlayerEntersBuilding ();
		}
	}
}
