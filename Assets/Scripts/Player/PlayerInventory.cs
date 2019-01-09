﻿using System.Collections;
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

	public void AddItemToInventory(Equippable equippableItem){

		int slot = GetFreeInventorySlot ();
		if (slot == -1) {
			Debug.LogWarning ("called additemtoinventory despite no slot being available, this shouldn't happen");
			return;
		}

		Debug.Log ("found slot " + slot + " for item " + equippableItem.id);

		inventory [slot] = equippableItem;
		UI.instance.slotImages [slot].name = equippableItem.id + " slot";
		UI.instance.slotImages [slot].sprite = itemIcons [equippableItem.id];
	}

	public void ScrollInput (float scrollValue){
		//value > 0 is forward, < 0 is backward
		Debug.Log("scroll value " + scrollValue);


		/* * 
		 * if the slot is empty (index == -1 and equippeditem.id == barehands), scrolling equips the next item 
		 * scrolling selects the next item in any case, the question is what happens to the active item then:
		 * drop it (if !carriable || inventory full), stow it in inventory (if carriable && inventory not full), nothing (if empty)
		 * 
		 * so first thing I do is decide which slot is "next" according to scroll direction, with nothing else done yet. This is to select the new
		 * item, not yet to place the old one anywhere 
		 * */
		int newIndex = currentlyActiveIndex;

		if (scrollValue > 0f) {
			++newIndex;
			if (newIndex >= inventory.Count) {
				newIndex = 0;
			} 
		} else if (scrollValue < 0f) {
			--newIndex;
			if (newIndex < 0) {
				newIndex = inventory.Count - 1;
			}
		}

		Debug.Log ("new slot: " + newIndex);


		int slotToFill = GetFreeInventorySlot ();

		if (player.currentlyEquippedItem.id == equippableItemID.BAREHANDS) {
			Debug.Log ("active slot was empty before");
			//if slot is empty, nothing
		} else if (player.currentlyEquippedItem.carriable && slotToFill != -1) {
			//if active item carriable && inventory not full, stow previously active item
			Debug.Log ("gonna store previous item in slot " + slotToFill);

			/*YOU WERE HERE*/

			/* 
			* UP NEXT:
			* Put item into inventory from here, make sure there's no duplicates
			*/

		} else {
			//if !carriable || inventory full, drop it
			Debug.Log ("item " + player.currentlyEquippedItem.id + " isn't carriable (" + !player.currentlyEquippedItem.carriable + ") or inventory is full (" + (slotToFill == -1) + ")");
			player.DropEquippedItem ();
		}

		if (inventory [newIndex] != null && inventory [newIndex].id != equippableItemID.BAREHANDS) {
			player.EquipAnItem (inventory [newIndex]);
			currentlyActiveIndex = newIndex; //set this once I don't need the old active index anymore, i.e. once the old item is dealt with (stowed, dropped, etc)
		} else {
			currentlyActiveIndex = -1;
		}
	}

	public void UpdateUIAfterDroppingItem(){
		UI.instance.activeSlotImage.name = "empty slot";
		UI.instance.activeSlotImage.sprite = null;
	}

	public void UpdateActiveSlot(Equippable newActiveItem, bool updateFromWithinInventory){
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
			Debug.Log ("no free slot, returning -1");
		} else {
			Debug.Log ("slot " + result + " is lowest free");
		}

		return result;
	} 
}
