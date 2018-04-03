using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour {

	public horseNeed needSatisfiedByThis;
	public float totalNeedValue;
	public float remainingNeedValue;
	public float consumptionRate;
	public bool destroyWhenEmpty;

	public virtual void Start(){
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
			if (destroyWhenEmpty) {
				StartCoroutine (Destroy ());
			}
		}
		UpdateValue ();
		return result;
	}

	public virtual void UpdateValue(){
	
	}

	private IEnumerator Destroy(){
		yield return new WaitForEndOfFrame ();
		GameObject.Destroy (gameObject);
	}
}
