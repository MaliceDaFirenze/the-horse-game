using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StallDirt : TimeDependentObject {

	public Gradient colorProgression;
	public MeshRenderer[] strawParts;
	public float dirtLevel = 0;
	private float dirtLevelIncreasePerDay = 0.15f;

	public override void StartNewDay(){
		dirtLevel += dirtLevelIncreasePerDay;

		if (dirtLevel > 1) {
			dirtLevel = 1;
		}

		UpdateStrawColor ();
	}

	public void Clean(){
		dirtLevel = 0;
		UpdateStrawColor ();
	}

	private void UpdateStrawColor(){
		foreach (MeshRenderer mr in strawParts) {
			mr.materials [0].color = colorProgression.Evaluate (dirtLevel);
		}
	}
}
