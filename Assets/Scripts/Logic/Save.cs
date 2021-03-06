﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {

	public int day;
	public int money;
	public int hayCartFill;
	public int strawCartFill;
	public int unlockedStallUnits;
	public Dictionary<int, int> buildingsUnderConstruction;
	public List<bool> paddockPartitions;
}
