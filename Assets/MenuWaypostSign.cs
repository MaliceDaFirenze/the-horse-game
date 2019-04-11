using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MenuWaypostSign : MonoBehaviour {
	MenuWaypost menu;
	public int signIndex;

	private void OnMouseDown (){
		menu.ClickSign (signIndex);

		//0: load main scene with forcenewgame disabled
		//1: load main scene with forcenewgame enabled
		//2: load credits scene
		//3: quit application
	}
}
