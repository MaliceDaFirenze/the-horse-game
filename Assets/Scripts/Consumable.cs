using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour {

	public horseNeed needSatisfiedByThis;
	public float totalNeedValue;
	public float remainingNeedValue;
	public float consumptionRate;

	private void Start(){
		remainingNeedValue = totalNeedValue;
	}

	public float PartialConsume(){
		float result;
		if (remainingNeedValue > consumptionRate) {
			result = consumptionRate;
			remainingNeedValue -= consumptionRate;
		} else {
			result = remainingNeedValue;
			remainingNeedValue = 0;
			StartCoroutine (Destroy ());
		}
		return result;
	}

	private IEnumerator Destroy(){
		yield return new WaitForEndOfFrame ();
		GameObject.Destroy (gameObject);
	}
}
