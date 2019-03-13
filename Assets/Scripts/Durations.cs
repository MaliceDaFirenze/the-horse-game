using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Durations : MonoBehaviour {

	public static List<int> stallConstructionDurations = new List<int>();

	public static void Setup(){
		stallConstructionDurations.Add (0);
		stallConstructionDurations.Add (2);
		stallConstructionDurations.Add (3);
		stallConstructionDurations.Add (3);
	}

	public static int GetStallConstructionDuration(int stallIndex){
		if (stallConstructionDurations.Count == 0) {
			Setup ();
		}

		int result = -1;

		if (stallIndex < stallConstructionDurations.Count) {
			result = stallConstructionDurations [stallIndex];
		} else {
			Debug.LogWarning ("no construction duration for stall index " + stallIndex);
		}

		return result;
	}
}
