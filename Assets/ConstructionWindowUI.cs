using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionWindowUI : MonoBehaviour {

	ConstructionBook constrBook;

	public Text priceForNextStall;
	public Text durationForNextStall;

	void Start(){
		constrBook = FindObjectOfType<ConstructionBook> ();
		if (constrBook == null) {
			Debug.LogWarning ("could not find construction book");
		}
	}

	public void CloseButtonPressed(){
		constrBook.CloseWindow ();
	}

	public void BuildButtonPressed(string buttonContent){
		constrBook.StartBuilding (buttonContent);
	}

}
