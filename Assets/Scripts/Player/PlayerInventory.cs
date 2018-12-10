using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

	//show currently equipped item in active slot
	//give equippables a "can be put into inventory" property (carriable)
	//start with sth like brush: equip, put away

	private int itemSlots;

	private List<Equippable> _inventory = new List<Equippable>();
	public List<Equippable> inventory {
		get { return _inventory; }
		private set { _inventory = value; }
	}

	private void Start(){
		itemSlots = UI.instance.slotImages.Length;
	}

	public void AddItemToInventory(Equippable equippableItem){
	
	}
}
