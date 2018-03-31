using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse_Interactable : Interactable {

	public Horse_Stats horseStats;

	public override void PlayerInteracts(/*Tool tool, */){
		base.PlayerInteracts ();
		horseStats.IncreaseHappiness (10);
	}
}
