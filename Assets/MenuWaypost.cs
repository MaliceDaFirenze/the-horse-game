using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuWaypost : MonoBehaviour {

	public void ClickSign(int index){
		//0: load main scene with forcenewgame disabled
		//1: load main scene with forcenewgame enabled
		//2: load credits scene
		//3: quit application
		switch (index){
		case 0:	
			GlobalGameLogic.instance.forceNewGame = false;
			SceneManager.LoadScene ("main");
			break;
		case 1: 
			GlobalGameLogic.instance.forceNewGame = true;
			SceneManager.LoadScene ("main");
			break;
		case 2: 
			SceneManager.LoadScene ("credits");
			break;
		case 3: 
			Application.Quit ();
			break;
		}
	}
}
