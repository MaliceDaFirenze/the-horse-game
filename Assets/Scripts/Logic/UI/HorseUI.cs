using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorseUI : MonoBehaviour {

	public Horse currentlyShowingHorse;

	public Image foodImage;
	public Image waterImage;
	public Image happinessImage;
	public Image hygieneImage;

	public GameObject uiElementsParent;

	public void ShowUIForHorse(Horse horse){
		uiElementsParent.SetActive (true);
		currentlyShowingHorse = horse;
		UpdateNeedsDisplay (horseNeed.FOOD, horse.Food);	
		UpdateNeedsDisplay (horseNeed.WATER, horse.Water);	
		UpdateNeedsDisplay (horseNeed.HAPPINESS, horse.Happiness);	
		UpdateNeedsDisplay (horseNeed.HYGIENE, horse.Hygiene);	
	}

	public void Hide(){
		currentlyShowingHorse = null;
		uiElementsParent.SetActive (false);
	}

	public void UpdateNeedsDisplay(horseNeed need, float newValue){
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
		case horseNeed.HYGIENE:
			imageToUpdate = hygieneImage;
			break;
		}
		
		imageToUpdate.fillAmount = newValue / 100;
		if (newValue >= 50) {
			imageToUpdate.color = Color.green;
		} else if (newValue >= 25) {
			imageToUpdate.color = Color.yellow;
		} else {
			imageToUpdate.color = Color.red;
		}
	}
}
