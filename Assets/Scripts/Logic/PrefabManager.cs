﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

	//---Particles---//
	public GameObject happinessParticles;

	//---Items---//
	public GameObject hayBale;


	//---Singleton---//
	private static PrefabManager _instance;
	public static PrefabManager instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<PrefabManager> ();
				if (_instance == null) {
					_instance = new GameObject("GlobalGameLogic", typeof(PrefabManager)).GetComponent<PrefabManager>();
				}
				DontDestroyOnLoad (_instance.gameObject);
			} 
			return _instance;
		}
	}
}
