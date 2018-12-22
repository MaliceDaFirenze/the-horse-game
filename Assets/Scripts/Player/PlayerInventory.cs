using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

	//show currently equipped item in active slot
	//give equippables a "can be put into inventory" property (carriable)
	//start with sth like brush: equip, put away

	private int itemSlots;
	private Dictionary <equippableItemID, Sprite> itemIcons = new Dictionary<equippableItemID, Sprite>();

	private int currentlyActiveIndex = -1;

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

	public void ScrollInput (float scrollValue){
		//value > 0 is forward, < 0 is backward
		Debug.Log("scroll value " + scrollValue);

		if (currentlyActiveIndex == -1){ //active slot is empty
			if (scrollValue > 0f) {
				++currentlyActiveIndex;

				int failsafe = 0;
				while (inventory [currentlyActiveIndex] == null || inventory [currentlyActiveIndex].id == equippableItemID.BAREHANDS) {
					++currentlyActiveIndex;
					if (currentlyActiveIndex >= inventory.Count){
						currentlyActiveIndex = 0;
					}
					++failsafe;
					if (failsafe > inventory.Count * 2) {
						Debug.LogWarning ("inventory is empty, do nothing");
						break;
					}
				}

				if (failsafe > inventory.Count * 2) {
					currentlyActiveIndex = -1;
					return;
				}

				Debug.Log ("new active slot " + currentlyActiveIndex + ", selected item " + inventory [currentlyActiveIndex].id);
			
			} else {
			/*	while (inventory.Count >= currentlyActiveIndex || inventory [currentlyActiveIndex] == null || inventory [currentlyActiveIndex].id == equippableItemID.BAREHANDS) {

				}*/
			}
		}
	}

	public void UpdateActiveSlot(){
		//can scroll if
			//active slot is empty
			//active slot contains carriable and there's space for it in inventory

		//otherwise, equippeditem is dropped now
	}
}
