using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDependantObject : MonoBehaviour {

	public bool excludeFirstDayUpdate;

	public virtual void StartNewDay(){
	
	}

	public virtual void IngameMinuteHasPassed(){
	
	}
}
