using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse_Interactable : Interactable {

	private Horse_Stats horseStats;
	private void Start(){
		horseStats = GetComponent<Horse_Stats> ();
	}

	public override void PlayerInteracts(Player player/*Tool tool, */){
		base.PlayerInteracts (player);
		horseStats.SatisfyNeed (horseNeed.HAPPINESS, 10);
	}
}
