using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBucket : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		GetComponent<Equippable> ().BeEquipped ();
		player.EquipAnItem (equippable);
	}
}

