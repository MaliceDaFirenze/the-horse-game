using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

	//show currently equipped item in active slot
	//give equippables a "can be put into inventory" property (carriable)
	//start with sth like brush: equip, put away

	private int itemSlots;
	private Dictionary <equippableItemID, Sprite> itemIcons = new Dictionary<equippableItemID, Sprite>();

	private int currentlyActiveIndex;

	private List<Equippable> _inventory = new List<Equippable>();
	public List<Equippable> inventory {
		get { return _inventory; }
		private set { _inventory = value; }
	}

	private void Start(){
		itemSlots = UI.instance.slotImages.Length;

		for (int i = 0; i < itemSlots; ++i) {
			inventory.Add (null);
		}


		equippableItemID[] carriableItemIds = new equippableItemID[] {
			equippableItemID.BRUSH,
			equippableItemID.BAREHANDS,
			equippableItemID.APPLE
		};

		for (int i = 0; i < carriableItemIds.Length; ++i) {
			itemIcons.Add(carriableItemIds[i], Resources.Load<Sprite>("ItemIcons/icon-" + carriableItemIds[i]));
		}
	}

	public void AddItemToInventory(Equippable equippableItem){
		//Find Free Slot
		int index;
		for (index = 0; index < itemSlots; ++index) {
			if (inventory [index] == null || inventory [index].id == equippableItemID.BAREHANDS) {
				//first slot that is free, use
				break;
			}
		}

		//if the last index wasn't free either, inventory is full, return
		if (inventory [index] != null && inventory [index].id != equippableItemID.BAREHANDS){
			Debug.Log ("inventory is full");
			return;
		}

		Debug.Log ("found slot " + index + " for item " + equippableItem.id);

		inventory [index] = equippableItem;
		UI.instance.slotImages [index].name = equippableItem.id + " slot";
		UI.instance.slotImages [index].sprite = itemIcons [equippableItem.id];

	}

	public void UpdateActiveSlot(){
	
	}
}
