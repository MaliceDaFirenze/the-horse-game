﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse_Stats : TimeDependentObject {

	public Horse horse;

	//---Needs---//
	[SerializeField]
	private float food;
	[SerializeField]
	private float water;
	[SerializeField]
	private float happiness;
	[SerializeField]
	private float hygiene;
	[SerializeField]
	private float energy;

	public float Food{
		get {
			return food;
		}
		private set { 
			if (value > needsMaximum) {
				food = needsMaximum;
			} else if (value < 0){
				food = 0;
			} else {
				food = value;
			}
			NeedsWereUpdated ();
		}
	}

	public float Water{
		get {
			return water;
		}
		private set { 
			if (value > needsMaximum) {
				water = needsMaximum;
			} else if (value < 0){
				water = 0;
			} else {
				water = value;
			}
			NeedsWereUpdated ();
		}
	}

	public float Happiness{
		get {
			return happiness;
		}
		private set { 
			if (value > needsMaximum) {
				happiness = needsMaximum;
			}  else if (value < 0){
				happiness = 0;
			}else {
				happiness = value;
			}
			NeedsWereUpdated ();
		}
	}

	public float Hygiene{
		get {
			return hygiene;
		}
		private set { 
			if (value > needsMaximum) {
				hygiene = needsMaximum;
			} else if (value < 0){
				hygiene = 0;
			} else {
				hygiene = value;
			}
			NeedsWereUpdated ();
		}
	}

	public float Energy{
		get {
			return energy;
		}
		private set { 
			if (value > needsMaximum) {
				energy = needsMaximum;
			} else if (value < 0){
				energy = 0;
			} else {
				energy = value;
			}
			NeedsWereUpdated ();
		}
	}

	private static float needsMaximum = 100; //food, thirst, happiness and hygiene all have the same maximum, unlike stats like stamina or sth
	public static float NeedsMaximum{
		get { 
			return needsMaximum;
		}
	}

	//---Decay---//
	//these should probably be influenced by stats, surroundings, gear, whatever (at some point)
	//all values per ingame minute
	private float foodDecay = 0.05f;
	private float waterDecay = 0.05f;
	private float happinessDecay = 0.03f;
	private float hygieneDecay = 0.02f;
	private float energyDecay = 0.02f;
	private Dictionary<horseGait, float> energyDecayMultiplierPerGait = new Dictionary<horseGait, float> ();
	private float energyDecayFoodMultiplier;
	private float energyDecayFoodMultiplierNeutral = 1f;
	private float energyDecayFoodMultiplierMax = -3f;

	//---Stats/Info---//
	//Age (die after x days)
	//speed, stamina, 

	//---references---//
	public Transform headBone;
	public Transform withersBone;
	public Transform poopSpawnPoint;
	private ParticleSystem heartParticles;
	private ParticleSystem dustParticles;
	private HorseUI horseUI;

	private void Start(){
		energyDecayMultiplierPerGait.Add (horseGait.STAND, 1f);
		energyDecayMultiplierPerGait.Add (horseGait.WALK, 2f);
		energyDecayMultiplierPerGait.Add (horseGait.TROT, 5f);
		energyDecayMultiplierPerGait.Add (horseGait.CANTER, 10f);
	}

	public void InitializeHorse(){
		//called from time logic, when new game starts

		Food = needsMaximum * 0.4f;
		Water = needsMaximum * 0.4f;
		Happiness = needsMaximum * 0.6f;
		Hygiene = needsMaximum * 0.7f;
		Energy = needsMaximum * 0.6f;
	}

	public override void StartNewDay(){
		base.StartNewDay ();
		Food -= foodDecay * 1000;
		Water -= waterDecay * 800;
		Happiness -= happinessDecay * 100;
		Hygiene -= hygieneDecay * 200;
		Energy = needsMaximum * 0.8f; //only if food & water the day before
	}

	public void AdjustEnergyMultiplier(bool horseJustAte){
		if (horseJustAte) {
			energyDecayFoodMultiplier = energyDecayFoodMultiplierMax;
		} else {
			energyDecayFoodMultiplier = energyDecayFoodMultiplierNeutral;
		}
	}
		
	public override void IngameMinuteHasPassed(){
		base.IngameMinuteHasPassed ();

		Food -= foodDecay;
		Water -= waterDecay;
		Happiness -= happinessDecay;
		Hygiene -= hygieneDecay;
		if (horse.horseBehavior.currentHorseGait == horseGait.STAND) {
			//set multiplier according to hunger/water levels. Positive multi (faster decay) when it's really low, negative multiplier (restore) for when the horse has just eaten

			Energy -= energyDecay * energyDecayFoodMultiplier;

		} else {
			Energy -= energyDecay * energyDecayMultiplierPerGait [horse.horseBehavior.currentHorseGait];
		}
	}

	public float GetNeedValue (horseNeed need) {
		switch (need) {
		case horseNeed.FOOD:
			return Food;
		case horseNeed.WATER:
			return Water;
		case horseNeed.HAPPINESS:
			return Happiness;
		case horseNeed.HYGIENE:
			return Hygiene;
		case horseNeed.ENERGY:
			return Energy;
		default: 
			Debug.LogWarning ("get invalid need");
			return Food;
		}
	}

	public void SatisfyNeed (horseNeed need, float value){
	
		switch (need) {
		case horseNeed.FOOD:
			Food += value;
			break;
		case horseNeed.WATER:
			Water += value;
			break;
		case horseNeed.HAPPINESS:
			Happiness += value;

			if (heartParticles == null) {
				heartParticles = Instantiate (PrefabManager.instance.happinessParticles, headBone.position, Quaternion.identity).GetComponent<ParticleSystem> ();
			} else {
				heartParticles.transform.position = headBone.position;
			}

			heartParticles.GetComponent<DeactivateAfterTime> ().Activate ();
			heartParticles.Play ();
			break;
		case horseNeed.HYGIENE:
			Hygiene += value;

			if (dustParticles == null) {
				dustParticles = Instantiate (PrefabManager.instance.dustParticles, withersBone.position, Quaternion.identity).GetComponent<ParticleSystem> ();
			} else {
				dustParticles.transform.position = withersBone.position;
			}

			dustParticles.GetComponent<DeactivateAfterTime> ().Activate ();
			dustParticles.Play ();
			break;

		case horseNeed.ENERGY:
			Energy += value;
			break;
		}
	}

	private void NeedsWereUpdated(){
		if (horseUI == null) {
			horseUI = FindObjectOfType<HorseUI> ();
		}
		if (horseUI.uiElementsParent.activeSelf && horseUI.currentlyShowingHorse == this) {
			horseUI.ShowUIForHorse (this);
		}

		horseUI.UpdateNeedsDisplay (horseNeed.ENERGY, Energy, true);
	}
}
