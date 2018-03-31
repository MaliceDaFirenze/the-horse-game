using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateAfterTime : MonoBehaviour {

	public float deactivateAfter;
	private WaitForSeconds wait;

	public void Activate(){
		gameObject.SetActive (true);
		StartCoroutine(Deactivate());	
	}

	private IEnumerator Deactivate(){
		if (wait == null) {
			wait = new WaitForSeconds (deactivateAfter);
		}

		yield return wait;
		gameObject.SetActive (false);
	}
}
