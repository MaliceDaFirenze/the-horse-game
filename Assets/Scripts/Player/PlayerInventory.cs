using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

	//show currently equipped item in active slot
	//give equippables a "can be put into inventory" property (carriable)
	//start with sth like brush: equip, put away

	private int itemSlots;
	private Dictionary <equippableItemID, Sprite> itemIcons = new Dictionary<equippableItemID, Sprite>();

	private List<equippableItemID> _inventory = new List<equippableItemID>();
	public List<equippableItemID> inventory {
		get { return _inventory; }
		private set { _inventory = value; }
	}

	private void Start(){
		itemSlots = UI.instance.slotImages.Length;

		for (int i = 0; i < itemSlots; ++i) {
			inventory.Add (equippableItemID.BAREHANDS);
		}

		itemIcons.Add(equippableItemID.BRUSH, Resources.Load<Sprite>("RewardIcons/icon-" + equippableItemID.BRUSH));
		itemIcons.Add(equippableItemID.BAREHANDS, Resources.Load<Sprite>("RewardIcons/icon-" + equippableItemID.BAREHANDS));
	}

	public void AddItemToInventory(Equippable equippableItem){
		//Find Free Slot
		int index;
		for (index = 0; index < itemSlots; ++index) {
			if (inventory [index] == equippableItemID.BAREHANDS) {
				//first slot that is free, use
				break;
			}
		}

		//if the last index wasn't free either, inventory is full, return
		if (inventory [index] != equippableItemID.BAREHANDS){
			Debug.Log ("inventory is full");
			return;
		}

		Debug.Log ("found slot " + index + " for item " + equippableItem.id);

		inventory [index] = equippableItem.id;
		UI.instance.slotImages [index].sprite = itemIcons [equippableItem.id];

	}
}
