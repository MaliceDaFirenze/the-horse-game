using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericInteractToEquip : Interactable {

	private void Start(){
		emptyHandsAction = "Pick up";
	}

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		if (player.currentlyEquippedItem.id == equippableItemID.BAREHANDS) {
			GetComponent<Equippable> ().BeEquipped ();
			player.EquipAnItem (equippable);
		}
	}
}
