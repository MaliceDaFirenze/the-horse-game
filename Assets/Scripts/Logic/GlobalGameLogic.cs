using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameLogic : MonoBehaviour {

	public bool forceNewGame;

	//---Singleton---//
	private static GlobalGameLogic _instance;
	public static GlobalGameLogic instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<GlobalGameLogic> ();
				if (_instance == null) {
					GameObject go = new GameObject ();
					_instance = go.AddComponent<GlobalGameLogic> ();
				}
				DontDestroyOnLoad (_instance.gameObject);
			} 
			return _instance;
		}
	}
}
