using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse_Mounted : MonoBehaviour {

	//what does this need to do?

	//establish links to behaviour, maybe stats, interactable, player...
	//input can be handled in Player script, but needs to be forwarded to this, where
	//the speed up/down and gait up/down logic is handled

	public void ReceivePlayerInput(Player player, dir input, Vector3 playerMovementVector){

		//no arrow input means no gait change. but movementvector with magnitude > 0 means keep ani of current gait. 
	
		Debug.Log ("player input: " + input);

		horseBehaviour.ChangeGaitByRiding (/*parameters?*/);
	}

	//references
	private Horse_Behavior _horseBehaviour;
	public Horse_Behavior horseBehaviour {
		get { 
			if (_horseBehaviour == null) {
				_horseBehaviour = GetComponent<Horse_Behavior> ();
			}
			return _horseBehaviour;
		}
	}
}
