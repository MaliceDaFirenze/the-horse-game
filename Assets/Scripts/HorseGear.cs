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

	public Transform girthPosOnHorse;
	public Transform girthPosHanging;
	public Transform girth;

	void Start(){
		anim = GetComponentInChildren<Animator> ();
	}
}
