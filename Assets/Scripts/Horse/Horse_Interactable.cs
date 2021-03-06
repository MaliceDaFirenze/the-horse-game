﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse_Interactable : Interactable {

	//References
	public Horse horse;

	private Equippable horseOnLeadEquippable;
	private Equippable mountedHorseEquippable;
	private Horse_RidingBehavior horseMounted;
	public Transform halterTransform;
	public Transform leadTransformLeading;
	public Transform leadTransformHanging;
	public Transform leadTransformTied;
	public Transform saddleTransform;
	public Transform bridleTransform;
	public Transform playerLeadingPos;
	public Transform playerMountedPos;

	//Gear
	public HorseGear headGear;  //halter, bit etc
	public HorseGear headGearAttachment;  //lead
	public HorseGear backGear; //saddle, blanket, saddlepad?   

	//definitions
	private Vector3 positionOnPost = new Vector3(3.2f, -0.4f, -11.4f);

	private void Start(){
		horseMounted = GetComponent<Horse_RidingBehavior> ();

		//get all equippable components on this GO, go through and assign them by id
		Equippable[] allEquippables = GetComponents<Equippable>();
		foreach (Equippable e in allEquippables) {
			switch (e.id) {
			case equippableItemID.HORSE_ON_LEAD:
				horseOnLeadEquippable = e;
				break;
			case equippableItemID.HORSE_MOUNTED:
				mountedHorseEquippable = e;
				break;
			}
		}
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
		case actionID.TAKE_SADDLE_WITH_PAD:
			TakeOffSaddleAndPad (player);
			break;
		case actionID.TAKE_HALTER_AND_LEAD:
			TakeOffHalterAndLead (player);
			break;
		case actionID.PUT_ON_HALTER_AND_LEAD:
			PutOnHalterAndLead (player);
			break;
		case actionID.PUT_ON_SADDLE_WITH_PAD:
			PutOnSaddleAndPad (player);
			break;
		case actionID.PUT_ON_BRIDLE:
			PutOnBridle (player);
			break;
		case actionID.TAKE_BRIDLE:
			TakeOffBridle (player);
			break;
		case actionID.LEAD_HORSE:
			StartLeadingHorse (player);
			break;
		case actionID.LEAD_BY_REINS:
			StartLeadingHorseByReins (player);
			break;
		case actionID.STOP_LEADING_BY_REINS:
			StopLeadingHorseByReins (player);
			break;
		case actionID.MOUNT_HORSE:
			MountHorse (player);
			break;
		case actionID.DISMOUNT_HORSE:
			Dismount (player);
			break;
		}
	}

	private void PetHorse(Player player){
		horse.horseStats.SatisfyNeed (horseNeed.HAPPINESS, 10);
	
	}

	private void BrushHorse (Player player){
		horse.horseStats.SatisfyNeed(horseNeed.HYGIENE, 20);
	}

	private void FeedHorse(Player player){
		Debug.Log ("feed horse, remainingNeedValue " + player.currentlyEquippedItem.GetComponent<Consumable>().remainingNeedValue);
		horse.horseStats.SatisfyNeed(horseNeed.FOOD, player.currentlyEquippedItem.GetComponent<Consumable>().remainingNeedValue);
		GameObject.Destroy (player.currentlyEquippedItem.gameObject);
		player.UnequipEquippedItem ();
		horse.horseBehavior.StartCoroutine (horse.horseBehavior.WaitToProduceManure ());
	}

	private void WaterHorse (Player player){
		horse.horseStats.SatisfyNeed(horseNeed.WATER, player.currentlyEquippedItem.GetComponent<Consumable>().remainingNeedValue);
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

		headGearAttachment.transform.position = leadTransformLeading.position;
		headGearAttachment.transform.rotation = leadTransformLeading.rotation;
		headGearAttachment.transform.SetParent (leadTransformLeading);
		GenericUtilities.EnableAllColliders (headGearAttachment.transform, false);

		StartLeadingHorse (player);
	}

	private void PutOnSaddleAndPad (Player player){
		backGear = player.currentlyEquippedItem.GetComponent<HorseGear> ();

		backGear.girth.position = backGear.girthPosOnHorse.position;
		backGear.girth.rotation = backGear.girthPosOnHorse.rotation;

		player.UnequipEquippedItem ();

		backGear.transform.position = saddleTransform.position;
		backGear.transform.rotation = saddleTransform.rotation;
		backGear.transform.SetParent (saddleTransform);
		GenericUtilities.EnableAllColliders (backGear.transform, false);
	} 
	private void TakeOffSaddleAndPad (Player player){
		Equippable saddleEquippable = backGear.GetComponent<Equippable> ();
		backGear.girth.position = backGear.girthPosHanging.position;
		backGear.girth.rotation = backGear.girthPosHanging.rotation;

		backGear = null;

		player.EquipAnItem(saddleEquippable);
		saddleEquippable.BeEquipped ();
	}

	private void PutOnBridle(Player player){
		headGear = player.currentlyEquippedItem.GetComponent<HorseGear> ();
		player.UnequipEquippedItem ();

		headGear.transform.position = bridleTransform.position;
		headGear.transform.rotation = bridleTransform.rotation;
		headGear.transform.SetParent (bridleTransform);
		headGear.anim.Play ("OnHorse");
		GenericUtilities.EnableAllColliders (headGear.transform, false);
	}

	private void TakeOffBridle(Player player){
		headGear.anim.Play ("Still");
		Equippable bridleEquippable = headGear.GetComponent<Equippable> ();

		headGear = null;

		player.EquipAnItem(bridleEquippable);
		bridleEquippable.BeEquipped ();
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

		horseOnLeadEquippable.EnableAllColliders (true);

		StopLeadingHorse (player);
	}

	private void TakeOffHalterAndLead(Player player){

		//find combined equippable, which is a child of Halter (but hierarchy changes, so it's not always the 0th child)
		Equippable combinedEquippable = null;
		Equippable[] equippablesInHalter = headGear.GetComponentsInChildren<Equippable> ();

		for (int i = 0; i < equippablesInHalter.Length; ++i) {
			if (equippablesInHalter [i].id == equippableItemID.HALTER_WITH_LEAD) {
				combinedEquippable = equippablesInHalter [i];
			}
		}

		headGearAttachment.anim.Play ("Still");
		headGearAttachment.transform.SetParent (combinedEquippable.transform);

		player.UnequipEquippedItem ();

		player.EquipAnItem(combinedEquippable);
		combinedEquippable.BeEquipped ();
		StopLeadingHorse (player);

		Debug.Log ("combined was equipped");

		headGear.transform.SetParent (combinedEquippable.transform);

		headGearAttachment = null;
		headGear = null;

		foreach (Transform child in combinedEquippable.transform) {
			//fix positions
			child.position = player.equippedItemPos.position;
			child.localEulerAngles = player.currentlyEquippedItem.equippedRotation;
			child.localPosition = child.GetComponent<Equippable>().equippedOffset;
		}

		horseOnLeadEquippable.EnableAllColliders (true, true);
		combinedEquippable.EnableAllColliders (false, true);
	}

	private void PutOnHalterAndLead(Player player){

		//store this here so that I can still access it once it's been unequipped
		Equippable combinedEquippable = player.currentlyEquippedItem;

		foreach (Transform child in player.currentlyEquippedItem.transform) {
			Debug.Log ("equipping " + child);
			horseGearType type = child.GetComponent<HorseGear> ().type;

			switch (type) {
			case horseGearType.HALTER:
				headGear = child.GetComponent<HorseGear> ();

				headGear.transform.position = halterTransform.position;
				headGear.transform.rotation = halterTransform.rotation;
				GenericUtilities.EnableAllColliders (headGear.transform, false);
				break;
			case horseGearType.LEAD:
				headGearAttachment = child.GetComponent<HorseGear> ();

				headGearAttachment.transform.position = leadTransformLeading.position;
				headGearAttachment.transform.rotation = leadTransformLeading.rotation;
				GenericUtilities.EnableAllColliders (headGearAttachment.transform, false);
				break;
			}
		}

		headGear.transform.SetParent (halterTransform);
		headGearAttachment.transform.SetParent (leadTransformLeading);

		player.UnequipEquippedItem ();

		combinedEquippable.transform.SetParent (headGear.transform);
		StartLeadingHorse (player);
	}

	private void StartLeadingHorse(Player player){
		headGearAttachment.anim.Play ("Leading");


		//set lead position (again) in case it was in hanging pos before
		headGearAttachment.transform.position = leadTransformLeading.position;
		headGearAttachment.transform.rotation = leadTransformLeading.rotation;
		headGearAttachment.transform.SetParent (leadTransformLeading);

		//horseOnLead as equippable
		horse.horseBehavior.PutHorseOnLead(true);

		//move player to leading position
		player.transform.position = playerLeadingPos.position;
		player.transform.rotation = playerLeadingPos.rotation;

		player.EquipAnItem(horseOnLeadEquippable, false);
	}

	private void StartLeadingHorseByReins(Player player){
		headGear.anim.Play ("Leading");

		horse.horseBehavior.PutHorseOnLead(true);
	
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

		//set lead and halter colliders to disabled again, bc they get enabled along with horse colliders when dropping the lead
		GenericUtilities.EnableAllColliders(headGear.transform, false);
		GenericUtilities.EnableAllColliders(headGearAttachment.transform, false);
	}

	public void TieHorseToPost(Player player){
		StopLeadingHorse (player);
		player.UnequipEquippedItem ();
		headGearAttachment.anim.Play ("Tied");
		transform.SetParent (player.nearestInteractable.transform);
		transform.localPosition = positionOnPost;
		headGearAttachment.transform.position = leadTransformTied.position;
		headGearAttachment.transform.rotation = leadTransformTied.rotation;
		headGearAttachment.transform.SetParent (leadTransformTied);
		horse.horseBehavior.TieHorseToPost (true);
		GenericUtilities.EnableAllColliders (transform, true);
	}

	public void TakeHorseFromPost(Player player){
		horse.horseBehavior.TieHorseToPost (false);
		player.EquipAnItem (horseOnLeadEquippable);
		StartLeadingHorse (player);
	}

	private void StopLeadingHorse(Player player){
		horse.horseBehavior.PutHorseOnLead(false);
	}
	public void StopLeadingHorseByReins (Player player){
		player.UnequipEquippedItem ();
		horse.horseBehavior.PutHorseOnLead(false);
		headGear.anim.Play ("OnHorse");
		GenericUtilities.EnableAllColliders (transform, true);
	}

	private void MountHorse(Player player){

		//move player to leading position
		player.transform.position = playerMountedPos.position;
		player.transform.rotation = playerMountedPos.rotation;
		player.playerModel.transform.SetParent (playerMountedPos);

		player.MountHorse (horseMounted);
		horseMounted.MountHorse (player);
		player.EquipAnItem(mountedHorseEquippable, false);

		//Disable horse's trigger
		GetComponent<Collider>().enabled = false;

		horse.horseBehavior.RidingHorse(true);
	}

	public void Dismount(Player player){
		player.UnequipEquippedItem ();
		horse.horseBehavior.RidingHorse(false);
		player.transform.position = playerLeadingPos.position;
		player.transform.rotation = playerLeadingPos.rotation;
		player.playerModel.transform.position = player.transform.position;
		player.playerModel.transform.rotation = player.transform.rotation;

		player.playerModel.transform.SetParent (player.transform);

		//enable all colliders (but only trigger really matters, right?
		GenericUtilities.EnableAllColliders (transform, true);

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

						currentlyRelevantActionIDs.Add (actionID.LEAD_HORSE);
						result.Add (InteractionStrings.GetInteractionStringById (actionID.LEAD_HORSE));
					} else {
						currentlyRelevantActionIDs.Add (actionID.TAKE_HALTER);
						result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_HALTER));
					}
				} else if (headGear.type == horseGearType.BRIDLE) {
					currentlyRelevantActionIDs.Add (actionID.TAKE_BRIDLE);
					result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_BRIDLE));
					currentlyRelevantActionIDs.Add (actionID.LEAD_BY_REINS);
					result.Add (InteractionStrings.GetInteractionStringById (actionID.LEAD_BY_REINS));
				}
			}
			if (backGear != null && backGear.type == horseGearType.SADDLE_WITH_PAD) {
				currentlyRelevantActionIDs.Add (actionID.TAKE_SADDLE_WITH_PAD);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_SADDLE_WITH_PAD));
			}

			if (backGear != null && headGear != null && backGear.type == horseGearType.SADDLE_WITH_PAD && headGear.type == horseGearType.BRIDLE) {
				currentlyRelevantActionIDs.Add (actionID.MOUNT_HORSE);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.MOUNT_HORSE));
			}

			break;
		case equippableItemID.STRAW:
			if (horse.horseStats.Food < Horse_Stats.NeedsMaximum) {
				currentlyRelevantActionIDs.Add (actionID.FEED_HORSE);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.FEED_HORSE));
			}
			break;
		case equippableItemID.HAY:
			if (horse.horseStats.Food < Horse_Stats.NeedsMaximum) {
				currentlyRelevantActionIDs.Add (actionID.FEED_HORSE);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.FEED_HORSE));
			}
			break;
		case equippableItemID.APPLE:
			if (horse.horseStats.Food < Horse_Stats.NeedsMaximum) {
				currentlyRelevantActionIDs.Add (actionID.FEED_HORSE);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.FEED_HORSE));
			}
			break;
		case equippableItemID.WATERBUCKET:
			if (horse.horseStats.Water < Horse_Stats.NeedsMaximum) {
				currentlyRelevantActionIDs.Add (actionID.WATER_HORSE);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.WATER_HORSE));
			}
			break;
		case equippableItemID.BRUSH:
			if (backGear == null && horse.horseStats.Hygiene < Horse_Stats.NeedsMaximum) {
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
				if (headGear.type == horseGearType.HALTER) {
					currentlyRelevantActionIDs.Add (actionID.TAKE_LEAD);
					result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_LEAD));

					currentlyRelevantActionIDs.Add (actionID.TAKE_HALTER_AND_LEAD);
					result.Add (InteractionStrings.GetInteractionStringById (actionID.TAKE_HALTER_AND_LEAD));
				} else if (headGear.type == horseGearType.BRIDLE) {
					currentlyRelevantActionIDs.Add (actionID.STOP_LEADING_BY_REINS);
					result.Add (InteractionStrings.GetInteractionStringById (actionID.STOP_LEADING_BY_REINS));
				}
			}
			break;
		case equippableItemID.HALTER_WITH_LEAD:
			if (headGear == null) {
				currentlyRelevantActionIDs.Add (actionID.PUT_ON_HALTER_AND_LEAD);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.PUT_ON_HALTER_AND_LEAD));
			}
			break;
		case equippableItemID.SADDLE_WITH_PAD:
			if (backGear == null) {
				currentlyRelevantActionIDs.Add (actionID.PUT_ON_SADDLE_WITH_PAD);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.PUT_ON_SADDLE_WITH_PAD));
			}
			break;
		case equippableItemID.BRIDLE:
			if (headGear == null) {
				currentlyRelevantActionIDs.Add (actionID.PUT_ON_BRIDLE);
				result.Add (InteractionStrings.GetInteractionStringById (actionID.PUT_ON_BRIDLE));
			}
			break;
		}
		return result;
	}
}
