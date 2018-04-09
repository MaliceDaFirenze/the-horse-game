using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse_Interactable : Interactable {

	private Horse_Stats horseStats;
	private Horse_Behavior horseBehaviour;

	private void Start(){
		horseStats = GetComponent<Horse_Stats> ();
		horseBehaviour = GetComponent<Horse_Behavior> ();
	}

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS:
			PetHorse (player);
			break;
		case equippableItemID.STRAW:
			if (horseStats.Food < Horse_Stats.NeedsMaximum) {
				FeedHorse (player);
			}
			break;
		case equippableItemID.WATERBUCKET:
			if (horseStats.Water < Horse_Stats.NeedsMaximum) {
				WaterHorse (player);
			}
			break;
		case equippableItemID.BRUSH:
			if (horseStats.Hygiene < Horse_Stats.NeedsMaximum) {
				BrushHorse (player);
			}
			break;
		default:
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

	public override string GetInteractionString (Player player)	{
		switch (player.currentlyEquippedItem.id) {
		case equippableItemID.BAREHANDS: 
			currentlyRelevantIntId = intID.PET_HORSE;
			return emptyHandsAction;
		case equippableItemID.STRAW:
			currentlyRelevantIntId = intID.FEED_HORSE;
		return "Feed";
		case equippableItemID.WATERBUCKET:
			currentlyRelevantIntId = intID.WATER_HORSE;
			return "Water";
		case equippableItemID.BRUSH:
			currentlyRelevantIntId = intID.BRUSH_HORSE;
			return "Brush";
		default: 
			return "";
		}
	}
}
