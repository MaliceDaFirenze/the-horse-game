using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionBookTime : TimeDependentObject {

	public override void StartNewDay(){
		GetComponent<ConstructionBook> ().NewDay ();
	}
}
