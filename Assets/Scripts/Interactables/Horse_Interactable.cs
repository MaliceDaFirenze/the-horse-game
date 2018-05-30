﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse_Interactable : Interactable {
	
	//References
	private Horse_Stats horseStats;
	private Horse_Behavior horseBehaviour;
	private Equippable horseOnLeadEquippable;
	public Transform halterTransform;
	public Transform leadTransformLeading;
	public Transform leadTransformHanging;
	public Transform playerLeadingPos;

	//Gear
	public HorseGear headGear;  //halter, bit etc
	public HorseGear headGearAttachment;  //lead
	public HorseGear backGear; //saddle, blanket, saddlepad?   

	private void Start(){
		horseStats = GetComponent<Horse_Stats> ();
		horseBehaviour = GetComponent<Horse_Behavior> ();
		horseOnLeadEquippable = GetComponent<Equippable> ();
	}

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (currentlyRelevantActionIDs [selectedInteractionIndex]) {
		case actionID.PET_HORSE:
			PetHorse (player);
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
		case actionID.PUT_ON_LEAD:
			PutOnLead (player);
			break;
		case actionID.TAKE_HALTER:
			TakeOffHalter (player);
			break;
		case actionID.TAKE_LEAD:
			TakeOffLead (player);
			break;
		case actionID.TAKE_HALTER_AND_LEAD:
			TakeOffHalterAndLead (player);
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

		player.UnequipEquippedItem ();

		headGear.transform.position = halterTransform.position;
		headGear.transform.rotation = halterTransform.rotation;
		headGear.transform.SetParent (halterTransform);
		GenericUtilities.EnableAllColliders (headGear.transform, false);
	}

	private void PutOnLead(Player player){
		headGearAttachment = player.currentlyEquippedItem.GetComponent<HorseGear> ();

		player.UnequipEquippedItem ();

		headGearAttachment.anim.Play ("Leading");
		headGearAttachment.transform.position = leadTransformLeading.position;
		headGearAttachment.transform.rotation = leadTransformLeading.rotation;
		headGearAttachment.transform.SetParent (leadTransformLeading);
		GenericUtilities.EnableAllColliders (headGearAttachment.transform, false);

		StartLeadingHorse (player);
	}

	private void TakeOffHalter(Player player){
		Equippable halterEquippable = headGear.GetComponent<Equippable> ();
		headGear = null;

		player.EquipAnItem(halterEquippable);
		halterEquippable.BeEquipped ();
	}

	private void TakeOffLead(Player player){
		headGearAttachment.anim.Play ("Still");

		Equippable leadEquippable = headGearAttachment.GetComponent<Equippable> ();
		headGearAttachment = null;
		player.UnequipEquippedItem ();

		player.EquipAnItem(leadEquippable);
		leadEquippable.BeEquipped ();

		StopLeadingHorse (player);
	}

	private void TakeOffHalterAndLead(Player player){
		Equippable combinedEquippable = headGear.transform.GetChild (0).GetComponent<Equippable> ();

		headGearAttachment.anim.Play ("Still");
		headGearAttachment.transform.SetParent (combinedEquippable.transform);

		headGearAttachment = null;
		headGear = null;
		player.UnequipEquippedItem ();

		player.EquipAnItem(combinedEquippable);
		combinedEquippable.BeEquipped ();

		StopLeadingHorse (player);
	}

	private void StartLeadingHorse(Player player){

		//horseOnLead as equippable
		horseBehaviour.PutHorseOnLead(true);

		//move player to leading position
		player.transform.position = playerLeadingPos.position;
		player.transform.rotation = playerLeadingPos.rotation;

		player.EquipAnItem(horseOnLeadEquippable, false);
	}

	public void LeadIsDropped(Player player){
		headGearAttachment.anim.Play ("Hanging");
		headGearAttachment.transform.position = leadTransformHanging.position;
		headGearAttachment.transform.rotation = leadTransformHanging.rotation;
		headGearAttachment.transform.SetParent (leadTransformHanging);
		StopLeadingHorse (player);
		//adjust position of lead for hanging pose
	}

	private void StopLeadingHorse(Player player){
		horseBehaviour.PutHorseOnLead(false);
	}

	public override List<string> DefineInteraction (Player player)	{
		List<string> result = new List<string> ();
		currentlyRelevantActionIDs.Clear ();

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS: 
			result.Add (InteractionStrings.GetInteractionStringById (actionID.PET_HORSE));
			currentlyRelevantActionIDs.Add (actionID.PET_HORSE);
			if (headGear != null) {
				if (headGear.type == horseGearType.HALTER) {
					if (headGearAttachment != null && headGearAttachment.type == horseGearType.LEAD) {
						currentlyRelevantActionIDs.Add (actionID.TAKE_HALTER_AND_LEAD);
						result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_HALTER_AND_LEAD));

						currentlyRelevantActionIDs.Add (actionID.TAKE_LEAD);
						result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_LEAD));
					}
					currentlyRelevantActionIDs.Add (actionID.TAKE_HALTER);
					result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_HALTER));
				}

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
			if (headGear != null && headGear.type == horseGearType.HALTER && headGearAttachment == null) {
				currentlyRelevantActionIDs.Add (actionID.PUT_ON_LEAD);
				result.Add(InteractionStrings.GetInteractionStringById(actionID.PUT_ON_LEAD));
			}
			break;
		case equippableItemID.HORSE_ON_LEAD:
			if (player.currentlyEquippedItem == horseOnLeadEquippable) { //i.e. it's this horse and not a different horse
				currentlyRelevantActionIDs.Add (actionID.TAKE_LEAD);
				result.Add(InteractionStrings.GetInteractionStringById(actionID.TAKE_LEAD));

				currentlyRelevantActionIDs.Add (actionID.TAKE_HALTER_AND_LEAD);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_HALTER_AND_LEAD));
			}
			break;
		}
		return result;
	}
}
