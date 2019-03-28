using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorseUI : MonoBehaviour {

	public Horse_Stats currentlyShowingHorse;

	public Image foodImage;
	public Image waterImage;
	public Image happinessImage;
	public Image energyImage;

	public GameObject uiElementsParent;

	public void ShowUIForHorse(Horse_Stats horse){
		uiElementsParent.SetActive (true);
		currentlyShowingHorse = horse;
		UpdateNeedsDisplay (horseNeed.FOOD, horse.Food);	
		UpdateNeedsDisplay (horseNeed.WATER, horse.Water);	
		UpdateNeedsDisplay (horseNeed.HAPPINESS, horse.Happiness);	
		UpdateNeedsDisplay (horseNeed.ENERGY, horse.Energy);	
	}

	public void Hide(){
		currentlyShowingHorse = null;
		uiElementsParent.SetActive (false);
	}

	public void UpdateNeedsDisplay(horseNeed need, float newValue, bool updateRidingUI = false){
		Image imageToUpdate = foodImage;

		switch (need) {
		case horseNeed.FOOD:
			imageToUpdate = foodImage;
			break;
		case horseNeed.WATER:
			imageToUpdate = waterImage;
			break;
		case horseNeed.HAPPINESS:
			imageToUpdate = happinessImage;
			break;
		case horseNeed.ENERGY:
			if (updateRidingUI) {
				imageToUpdate = UI.instance.ridingUI.energyBar;
			} else {
				imageToUpdate = energyImage;
			}
			break;
		}
		
		imageToUpdate.fillAmount = newValue / 100;
		if (newValue >= 40) {
			imageToUpdate.color = Color.green;
		} else if (newValue >= 20) {
			imageToUpdate.color = Color.yellow;
		} else {
			imageToUpdate.color = Color.red;
		}
	}
}
