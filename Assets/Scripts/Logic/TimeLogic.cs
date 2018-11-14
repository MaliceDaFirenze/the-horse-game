using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class TimeLogic : MonoBehaviour {

	//references
	private Player player;

	public int day;

	private float dayStartTime;
	private float dayStartTimeAddition = 6; //in "hours"
	public float timePassedToday;
	private TimeSpan timePassedTodaySpan;
	private bool timeCounting;

	//UI
	public Text dateText;
	public Image blackScreen;

	//Time-dependant objects
	private int lastMinuteCount;

	void Start () {
		player = FindObjectOfType<Player> ();
		NewGame (); //only if no save game?
	}

	private void NewGame(){
		day = 1;
		StartNewDay ();
	}

	public void EndDay(){
		player.allowPlayerInput = false;
		timeCounting = false;
		++day;

		SaveGame ();

		StartCoroutine (DayEndFade ());
	}

	private IEnumerator DayEndFade(){
		blackScreen.CrossFadeAlpha (255f, 1f, true);
		yield return new WaitForSeconds (2);
		blackScreen.CrossFadeAlpha (0f, 1f, true);
		StartNewDay ();
	}

	private void StartNewDay(){
		//Time
		timePassedToday = 0;
		dayStartTime = Time.time;
		timeCounting = true;

		//Player
		player.allowPlayerInput = true;

		//Time-dependant objects
		TimeDependentObject[] allTimeDependantObjects = FindObjectsOfType<TimeDependentObject> ();
		for (int i = 0; i < allTimeDependantObjects.Length; ++i) {
			if (!allTimeDependantObjects [i].excludeFirstDayUpdate || day != 1) {
				allTimeDependantObjects [i].StartNewDay ();
			}
		}
	}

	void Update () {
		if (timeCounting) {
			timePassedToday = (Time.time - dayStartTime) * 0.65f;
			timePassedTodaySpan = TimeSpan.FromSeconds (timePassedToday);
			dateText.text = "Day " + day + " - " + string.Format("{0:00}:{1:00}", (timePassedTodaySpan.Minutes + dayStartTimeAddition), timePassedTodaySpan.Seconds);

			if (lastMinuteCount != timePassedTodaySpan.Seconds) {
				TimeDependentObject[] allTimeDependantObjects = FindObjectsOfType<TimeDependentObject> ();
				for (int i = 0; i < allTimeDependantObjects.Length; ++i) {
					allTimeDependantObjects [i].IngameMinuteHasPassed ();
				}

				lastMinuteCount = timePassedTodaySpan.Seconds;
			}
		}
	}

	private void SaveGame(){
		Save save = CreateSaveGameObject ();

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/gamesave.save");
		bf.Serialize (file, save);
		file.Close ();

		Debug.Log ("game saved");
	}


	private Save CreateSaveGameObject(){
		Save save = new Save ();

		save.day = day;
		//save.money = something?

		return save;
	}
}
