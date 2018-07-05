using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum horseGearType{
	HALTER,
	LEAD,
	BRIDLE,
	SADDLE_WITH_PAD
}

public class HorseGear : MonoBehaviour {
	public horseGearType type;
	public Animator anim;

	void Start(){
		anim = GetComponentInChildren<Animator> ();
	}
}
