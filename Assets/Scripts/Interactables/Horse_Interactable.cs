using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse_Interactable : Interactable {
	
	//References
	private Horse_Stats horseStats;
	private Horse_Behavior horseBehaviour;
	public Transform halterTransform;

	//Gear
	public HorseGear headGear;  //halter, bit etc
	public HorseGear headGearAttachment;  //lead
	public HorseGear backGear; //saddle, blanket, saddlepad?   

	private void Start(){
		horseStats = GetComponent<Horse_Stats> ();
		horseBehaviour = GetComponent<Horse_Behavior> ();
	}

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.PET_HORSE:
			PetHorse (player);
			break;
		case actionID.TAKE_HALTER:
			TakeOffHalter (player);
			break;
		case actionID.FEED_HORSE:
			FeedHorse (player);
			break;
		case actionID.WATER_HORSE:
			WaterHorse (player);
			break;
		case actionID.BRUSH_HORSE:
			BrushHorse (player);
			break;
		case actionID.PUT_ON_HALTER:
			PutOnHalter (player);
			break;
		}
	}

	private void PetHorse(Player player){
		horseStats.SatisfyNeed (horseNeed.HAPPINESS, 10);
	
	}

	private void BrushHorse (Player player){
		horseStats.SatisfyNeed(horseNeed.HYGIENE, 20);
	}

	private void FeedHorse(Player player){
		Debug.Log ("feed horse, remainingNeedValue " + player.currentlyEquippedItem.GetComponent<Consumable>().remainingNeedValue);
		horseStats.SatisfyNeed(horseNeed.FOOD, player.currentlyEquippedItem.GetComponent<Consumable>().remainingNeedValue);
		GameObject.Destroy (player.currentlyEquippedItem.gameObject);
		player.UnequipEquippedItem ();
		horseBehaviour.StartCoroutine (horseBehaviour.WaitToProduceManure ());
	}

	private void WaterHorse (Player player){
		horseStats.SatisfyNeed(horseNeed.WATER, player.currentlyEquippedItem.GetComponent<Consumable>().remainingNeedValue);
		player.currentlyEquippedItem.GetComponent<Consumable> ().remainingNeedValue = 0;
		player.currentlyEquippedItem.GetComponent<WaterBucket_Consumable> ().UpdateValue ();
	}

	private void PutOnHalter(Player player){
		headGear = player.currentlyEquippedItem.GetComponent<HorseGear> ();
		headGear.transform.position = halterTransform.position;
		headGear.transform.rotation = halterTransform.rotation;
		headGear.transform.SetParent (halterTransform);
		GenericUtilities.EnableAllColliders (headGear.transform, false);
		player.UnequipEquippedItem ();
	}

	private void PutOnLead(Player player){

	}

	private void TakeOffHalter(Player player){
		Equippable halterEquippable = headGear.GetComponent<Equippable> ();
		headGear = null;

		player.EquipAnItem(halterEquippable);
		halterEquippable.BeEquipped ();
	}

	private void TakeOffLead(Player player){

	}


	public override List<string> DefineInteraction (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear ();

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS: 
			result.Add (InteractionStrings.GetInteractionStringById (actionID.PET_HORSE));
			currentlyRelevantActionIDs.Add (actionID.PET_HORSE);
			if (headGear != null) {
				currentlyRelevantActionIDs.Add (actionID.TAKE_HALTER);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_HALTER));
			}
			break;
		case equippableItemID.STRAW:
			if (horseStats.Food < Horse_Stats.NeedsMaximum) {
				currentlyRelevantActionIDs.Add (actionID.FEED_HORSE);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.FEED_HORSE));
			}
			break;
		case equippableItemID.WATERBUCKET:
			if (horseStats.Water < Horse_Stats.NeedsMaximum) {
				currentlyRelevantActionIDs.Add (actionID.WATER_HORSE);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.WATER_HORSE));
			}
			break;
		case equippableItemID.BRUSH:
			if (backGear == null && horseStats.Hygiene < Horse_Stats.NeedsMaximum) {
				currentlyRelevantActionIDs.Add (actionID.BRUSH_HORSE);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.BRUSH_HORSE));
			}
			break;
		case equippableItemID.HALTER:
			if (headGear == null) {
				currentlyRelevantActionIDs.Add (actionID.PUT_ON_HALTER);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.PUT_ON_HALTER));
			}
			break;
		case equippableItemID.LEAD:
			if (headGear != null && headGear.type == horseGearType.HALTER) {
				currentlyRelevantActionIDs.Add (actionID.PUT_ON_LEAD);
				result.Add(InteractionStrings.GetInteractionStringById(actionID.PUT_ON_LEAD));
			}
			break;
		}


		return result;
	}
}
