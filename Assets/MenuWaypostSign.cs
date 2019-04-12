using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MenuWaypostSign : MonoBehaviour {
	MenuWaypost menu;
	public int signIndex;

	private void Start(){
		menu = transform.parent.GetComponent<MenuWaypost> ();
	}

	private void OnMouseDown (){
		menu.ClickSign (signIndex);
	
	}
}
