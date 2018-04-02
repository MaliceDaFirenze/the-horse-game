using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		FindObjectOfType<TimeLogic> ().EndDay ();
	}
}
