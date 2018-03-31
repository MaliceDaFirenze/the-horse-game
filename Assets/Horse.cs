using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum horseNeed {
	FOOD,
	WATER,
	HAPPINESS,
	HYGIENE
}

public class Horse : TimeDependantObject {

	//---Needs---//
	[SerializeField]
	private float food;
	[SerializeField]
	private float water;
	[SerializeField]
	private float happiness;
	[SerializeField]
	private float hygiene;

	public float Food{
		get {
			return food;
		}
		private set { 
			food = value;
			NeedsWereUpdated ();
		}
	}

	public float Water{
		get {
			return water;
		}
		private set { 
			water = value;
			NeedsWereUpdated ();
		}
	}

	public float Happiness{
		get {
			return happiness;
		}
		private set { 
			happiness = value;
			NeedsWereUpdated ();
		}
	}

	public float Hygiene{
		get {
			return hygiene;
		}
		private set { 
			hygiene = value;
			NeedsWereUpdated ();
		}
	}

	private float needsMaximum = 100; //food, thirst, happiness and hygiene all have the same maximum, unlike stats like stamina or sth

	//---Decay---//
	//these should probably be influenced by stats, surroundings, gear, whatever (at some point)
	//all values per ingame minute
	private float foodDecay = 0.05f;
	private float waterDecay = 0.05f;
	private float happinessDecay = 0.03f;
	private float hygieneDecay = 0.02f;

	//---Stats/Info---//
	//Age (die after x days)
	//speed, stamina, 

	//---references---//
	public Transform headBone;
	private ParticleSystem heartParticles;
	private HorseUI horseUI;

	private void Start(){
		InitializeHorse (); //later, obvs don't call this from start anymore
	}

	private void InitializeHorse(){
		Food = needsMaximum * 0.8f;
		Water = needsMaximum * 0.8f;
		Happiness = needsMaximum * 0.6f;
		Hygiene = needsMaximum * 0.9f;
	}

	public override void StartNewDay(){
		base.StartNewDay ();
		Food -= foodDecay * 1000;
		Water -= waterDecay * 800;
		Happiness -= happinessDecay * 100;
		Hygiene -= hygieneDecay * 200;
	}
		
	public override void IngameMinuteHasPassed(){
		base.IngameMinuteHasPassed ();

		Food -= foodDecay;
		Water -= waterDecay;
		Happiness -= happinessDecay;
		Hygiene -= hygieneDecay;
	}

	public void IncreaseFood(float value){
		Food += value;
	}

	public void IncreaseWater(float value){
		Water += value;
	}

	public void IncreaseHappiness(float value){
		Happiness += value;

		if (heartParticles == null) {
			heartParticles = Instantiate (PrefabManager.instance.happinessParticles, headBone.position, Quaternion.identity).GetComponent<ParticleSystem> ();
		}

		heartParticles.GetComponent<DeactivateAfterTime> ().Activate ();
		heartParticles.Play ();
	}

	public void IncreaseHygiene(float value){
		Hygiene += value;
	}

	private void NeedsWereUpdated(){
		if (horseUI == null) {
			horseUI = FindObjectOfType<HorseUI> ();
		}
		if (horseUI.uiElementsParent.activeSelf && horseUI.currentlyShowingHorse == this) {
			horseUI.ShowUIForHorse (this);
		}
	}
}
