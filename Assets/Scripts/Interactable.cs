using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum actionID {
	_EMPTYSTRING,
	OPEN_CLOSE_DOOR,
	NO_SEQUENCE,
	BRUSH_HORSE,
	PET_HORSE,
	FEED_HORSE,
	WATER_HORSE,
	CLEAN_MANURE,
	FILL_BUCKET,
	EMPTY_PITCHFORK,
	PUSH_WHEELBARROW,
	EMPTY_WHEELBARROW,
	PUT_AWAY_STRAW,
	TAKE_STRAW,
	HANG_UP_HALTER,
	HANG_UP_LEAD,
	TAKE_HALTER,
	TAKE_LEAD,
	TAKE_HALTER_AND_LEAD,
	PUT_ON_HALTER,
	PUT_ON_LEAD,
	PUT_ON_HALTER_AND_LEAD,
	HANG_UP_HALTER_AND_LEAD,
	LEAD_HORSE,
	TIE_HORSE_TO_POST,
	TAKE_HORSE_FROM_POST,
	TAKE_SADDLE_WITH_PAD,
	PUT_ON_SADDLE_WITH_PAD,
	HANG_UP_SADDLE_WITH_PAD,
	TAKE_BRIDLE,
	HANG_UP_BRIDLE,
	PUT_ON_BRIDLE,
	LEAD_BY_REINS,
	STOP_LEADING_BY_REINS,
	MOUNT_HORSE,
	DISMOUNT_HORSE,
	REARRANGE_OBSTACLES,
	RESTOCK_CART,
	REFILL_CART,
	TALK_TO,
	SELL,
	PUT_INTO_POCKET,
	PICK_UP
}

public class Interactable : MonoBehaviour {

	public string emptyHandsAction;
	public Equippable equippable;
	public Consumable consumable;

	public bool pickUpable;

	public List<actionID> currentlyRelevantActionIDs = new List<actionID>(); //changes based on equippable, get set on enter int trigger
	public int selectedInteractionIndex;
	public dir[] arrowInputRequired; //changes with actionID
	public int nextArrowIndexToInput;

	private void Awake(){
		if (GetComponent<Collider> () == null) {
			Debug.LogWarning ("Interactable " + name + " does not have collider!");
		} else if (!GetComponent<Collider> ().isTrigger) {
			Debug.LogWarning ("Interactable " + name + " collider is not trigger!");
		}

		equippable = GetComponent<Equippable> ();
		consumable = GetComponent<Consumable> ();
	}

	public void PlayerPressesArrow(Player player, dir input){
		if (input == arrowInputRequired [nextArrowIndexToInput]) {
			//Debug.Log ("arrow correct");
			++nextArrowIndexToInput;
			UI.instance.UpdateArrows (nextArrowIndexToInput);
			if (arrowInputRequired.Length == nextArrowIndexToInput) {
				PlayerInteracts (player);
				UI.instance.ArrowSequenceComplete ();
			}
		} else {
			//Debug.Log ("arrow false");
			nextArrowIndexToInput = 0;
			UI.instance.UpdateArrows (nextArrowIndexToInput);
			UI.instance.ShakeArrows ();
		}
	}

	public virtual void PlayerInteracts(Player player){
		nextArrowIndexToInput = 0;

		//may have to add the generic interact to equip stuff here? 

		//Debug.Log ("base interact");
	}

	public virtual void PlayerEntersIntTrigger(){
		nextArrowIndexToInput = 0;
		selectedInteractionIndex = 0;
	}

	public virtual List<string> DefineInteraction(Player player){
		currentlyRelevantActionIDs.Clear ();
		List<string> result = new List<string> ();

		if (player.currentlyEquippedItem.id == equippableItemID.BAREHANDS) {

			if (pickUpable) {
				currentlyRelevantActionIDs.Add (actionID.PICK_UP);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.PICK_UP));
			}


			if (equippable != null && equippable.carriable && player.inventory.GetFreeInventorySlot() != -1) {
				currentlyRelevantActionIDs.Add (actionID.PUT_INTO_POCKET);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.PUT_INTO_POCKET));
			}
		}
	
		//result.Add (emptyHandsAction);
		//currentlyRelevantActionIDs.Add (actionID.NO_SEQUENCE);
		return result;
	}
}
