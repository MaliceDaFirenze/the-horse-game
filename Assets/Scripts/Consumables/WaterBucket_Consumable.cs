using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBucket_Consumable : Consumable {

	public Transform waterModel;
	private Vector3 originalScale;

	public override void Start(){
		base.Start ();
		originalScale = waterModel.localScale;	
	}

	public override void UpdateValue(){
		base.UpdateValue ();

		waterModel.localScale = originalScale * (remainingNeedValue / totalNeedValue);
	}
}
