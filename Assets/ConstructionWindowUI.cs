using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionWindowUI : MonoBehaviour {

	ConstructionBook constrBook;

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
