using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private void Start(){
		InitializeHorse (); //later, obvs don't call this from start anymore
	}

	private void InitializeHorse(){
		food = needsMaximum * 0.8f;
		water = needsMaximum * 0.8f;
		happiness = needsMaximum * 0.6f;
		hygiene = needsMaximum * 0.9f;
	}

	public override void StartNewDay(){
		base.StartNewDay ();
		food -= foodDecay * 1000;
		water -= waterDecay * 800;
		happiness -= happinessDecay * 100;
		hygiene -= hygieneDecay * 200;
	}

	public override void IngameMinuteHasPassed(){
		base.IngameMinuteHasPassed ();

		food -= foodDecay;
		water -= waterDecay;
		happiness -= happinessDecay;
		hygiene -= hygieneDecay;
	}

	public void IncreaseFood(){
	
	}

	public void IncreaseWater(){

	}

	public void IncreaseHappiness(){

	}

	public void IncreaseHygiene(){

	}
}
