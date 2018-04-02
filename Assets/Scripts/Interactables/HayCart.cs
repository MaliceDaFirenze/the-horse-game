using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayCart : Interactable {

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		GameObject newHayBale = Instantiate (PrefabManager.instance.hayBale, player.equippedItemPos.position, player.equippedItemPos.rotation) as GameObject;

		Equippable equippable = newHayBale.GetComponent<Equippable> ();
		equippable.Initialize ();

		player.EquipAnItem (equippable);
	}
}
