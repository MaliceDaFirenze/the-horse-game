using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

	//show currently equipped item in active slot
	//give equippables a "can be put into inventory" property (carriable)
	//start with sth like brush: equip, put away

	private int itemSlots;
	private Dictionary <equippableItemID, Sprite> itemIcons = new Dictionary<equippableItemID, Sprite>();
	private Player player;

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

		foreach (equippableItemID id in System.Enum.GetValues(typeof(equippableItemID))) {
			itemIcons.Add(id, Resources.Load<Sprite>("ItemIcons/icon-" + id));
		}
		player = GetComponent<Player>();
	}

	public void AddItemToInventory(Equippable equippableItem, int overwriteSlot = -1){
		int slot = overwriteSlot;

		if (overwriteSlot == -1) {
			slot = GetFreeInventorySlot ();
			if (slot == -1) {
				Debug.LogWarning ("called additemtoinventory despite no slot being available, this shouldn't happen");
				return;
			}
		}

		Debug.Log ("using slot " + slot + " for item " + equippableItem.id);

		inventory [slot] = equippableItem;
		UI.instance.slotImages [slot].name = equippableItem.id + " slot";
		UI.instance.slotImages [slot].sprite = itemIcons [equippableItem.id];
	}

	public void RemoveActiveItemFromInventory(Equippable item){

		//Debug.Log ("removing item " + item.id);

		if (inventory.Contains (item)) { //can also be only in active slot

			int slot = inventory.IndexOf (item);
			inventory [slot] = null;

			inventory [slot] = null;
			UI.instance.slotImages [slot].name = "Slot";
			UI.instance.slotImages [slot].sprite = null;
		}
		currentlyActiveIndex = -1;
		SetActiveSlotUIToEmpty ();


		//DEBUG STRING ONLY
		string debugStringInventory = "";
		for (int i = 0; i < inventory.Count; ++i) {
			debugStringInventory += i + ". ";
			if (inventory[i] == null) {
				debugStringInventory += "empty\n";
			} else {
				debugStringInventory += inventory[i].id + ", " + inventory[i].name + "\n";
			}
		}
		//

		//Debug.Log ("inventory AFTER REMOVING:\n" + debugStringInventory);

	}

	public void ScrollInput (float scrollValue){
		//value > 0 is forward, < 0 is backward
		//Debug.Log("NEW INPUT scroll value " + scrollValue);


		/* * 
		 * if the slot is empty (index == -1 and equippeditem.id == barehands), scrolling equips the next item 
		 * scrolling selects the next item in any case, the question is what happens to the active item then:
		 * drop it (if !carriable || inventory full), stow it in inventory (if carriable && inventory not full), nothing (if empty)
		 * 
		 * so first thing I do is decide which slot is "next" according to scroll direction, with nothing else done yet. This is to select the new
		 * item, not yet to place the old one anywhere 
		 * */
		int newIndex = currentlyActiveIndex;

		if (scrollValue < 0f) {
			++newIndex;
			if (newIndex >= inventory.Count) {
				newIndex = 0;
			} 
		} else if (scrollValue > 0f) {
			--newIndex;
			if (newIndex < 0) {
				newIndex = inventory.Count - 1;
			}
		}

		//DEBUG STRING ONLY
		string debugStringInventory = "";
		for (int i = 0; i < inventory.Count; ++i) {
			debugStringInventory += i + ". ";
			if (inventory[i] == null) {
				debugStringInventory += "empty\n";
			} else {
				debugStringInventory += inventory[i].id + ", " + inventory[i].name + "\n";
			}
		}
		//

		//Debug.Log ("new slot: " + newIndex + ". currently equipped item: " + player.currentlyEquippedItem.id + " inventory BEFORE: \n" + debugStringInventory);
		/*if (inventory [0] != null) {
			Debug.Log ("item in slot 0 here: " + inventory [0].id);
		} else {
			Debug.Log ("slot 0 = null");
		}*/

		if (!inventory.Contains (player.currentlyEquippedItem)) {

			int slotToFill = GetFreeInventorySlot ();

			if (player.currentlyEquippedItem.id == equippableItemID.BAREHANDS) {
			//	Debug.Log ("active slot was empty before");
				//if slot is empty, nothing
			} else if (player.currentlyEquippedItem.carriable && slotToFill != -1) {
				if (!inventory.Contains (player.currentlyEquippedItem)) {

					//if active item carriable && inventory not full, stow previously active item
					//Debug.Log ("gonna store previous item (" + player.currentlyEquippedItem.id + ") in slot " + slotToFill);

					AddItemToInventory (player.currentlyEquippedItem, slotToFill);
				} 
				
				SetActiveSlotUIToEmpty ();
				player.currentlyEquippedItem.gameObject.SetActive (false);
				
			} else {
				//if !carriable || inventory full, drop it
				//Debug.Log ("item " + player.currentlyEquippedItem.id + " isn't carriable (" + !player.currentlyEquippedItem.carriable + ") or inventory is full (" + (slotToFill == -1) + ")");
				player.DropEquippedItem ();
			}

		} else {
			//Debug.Log ("NO inv contains bracket. set active slot to empty");
			//Debug.Log ("item in slot 0 here: " + inventory [0].id);
			SetActiveSlotUIToEmpty ();
			player.currentlyEquippedItem.gameObject.SetActive (false);
		}

		/*if (inventory [0] != null) {
			Debug.Log ("item in slot 0 here: " + inventory [0].id);
		} else {
			Debug.Log ("slot 0 = null");
		}*/


		if (inventory [newIndex] != null && inventory [newIndex].id != equippableItemID.BAREHANDS) {
			player.EquipAnItem (inventory [newIndex]);
			inventory [newIndex].gameObject.SetActive (true);
			//Debug.Log (inventory [newIndex].id + " selected, in slot " + newIndex);
		} else {
			player.UnequipEquippedItem (false, false);
			//Debug.Log ("no / empty item selected, in slot " + newIndex);
		}

		/*if (inventory [0] != null) {
			Debug.Log ("item in slot 0 here: " + inventory [0].id);
		} else {
			Debug.Log ("slot 0 = null");
		}*/


		currentlyActiveIndex = newIndex; //set this once I don't need the old active index anymore, i.e. once the old item is dealt with (stowed, dropped, etc)

		for (int i = 0; i < UI.instance.slotImages.Length; ++i) {
			if (i == currentlyActiveIndex) {
				UI.instance.frameImages [i].enabled = true;
			} else {
				UI.instance.frameImages [i].enabled = false;
			}
		}

		if (currentlyActiveIndex != -1) {
			UI.instance.activeFrameImage.enabled = false;
		}

		//DEBUG STRING ONLY
		debugStringInventory = "";
		for (int i = 0; i < inventory.Count; ++i) {
			debugStringInventory += i + ". ";
			if (inventory[i] == null) {
				debugStringInventory += "empty\n";
			} else {
				debugStringInventory += inventory[i].id + ", " + inventory[i].name + "\n";
			}
		}
		//

		//Debug.Log ("inventory AFTER:\n" + debugStringInventory);
	}

	public void SetActiveSlotUIToEmpty(){
		UI.instance.activeSlotImage.name = "empty slot";
		UI.instance.activeSlotImage.sprite = null;
	}

	public void UpdateActiveSlot(Equippable newActiveItem, bool updateFromWithinInventory){
	//	Debug.Log ("UpdateActiveSlot, newActiveItem id: " + newActiveItem.id);
		//can scroll if
			//active slot is empty
			//active slot contains carriable and there's space for it in inventory

		//otherwise, equippeditem is dropped now

		//I equip an item
		//it is put into the active slot
		//none of the inventory items are actually "active" right now, so active index is set to -1
		if (!updateFromWithinInventory) {
			currentlyActiveIndex = -1;
		}

		UI.instance.activeSlotImage.name = newActiveItem.id + " slot";
		UI.instance.activeSlotImage.sprite = itemIcons [newActiveItem.id];
		UI.instance.activeFrameImage.enabled = true;
	}

	public int GetFreeInventorySlot(){
		int result;
		bool slotIsFree = false;

		for (result = 0; result < inventory.Count; ++result){
			if (inventory [result] == null || inventory [result].id == equippableItemID.BAREHANDS) {
				slotIsFree = true;
				break;
			}
		}

		if (!slotIsFree) {
			result = -1;
			//Debug.Log ("no free slot, returning -1");
		} else {
			//Debug.Log ("slot " + result + " is lowest free");
		}

		return result;
	} 
}
