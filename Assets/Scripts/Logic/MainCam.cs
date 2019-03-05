using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour {

	private Player player;

	public Transform closePos;
	public Transform farPos;
	private bool isInBuilding;
	private Camera cam;

	private void Start(){
		player = FindObjectOfType<Player> ();
		cam = GetComponentInChildren<Camera> ();
	}

	private void Update(){
		transform.position = player.transform.position;
	}

	public void PlayerEntersBuilding(){
		//cam.transform.position = closePos.position;
		if (!isInBuilding){
			isInBuilding = true;
			LeanTween.cancelAll();
			LeanTween.move (cam.gameObject, closePos.position, 0.5f);
			LeanTween.rotate (cam.gameObject, closePos.eulerAngles, 0.45f);
		}
	}

	public void PlayerExitsBuilding(){
		//cam.transform.position = farPos.position;
		if (isInBuilding){
			isInBuilding = false;
			LeanTween.cancelAll();
			LeanTween.move (cam.gameObject, farPos.position, 0.5f);
			LeanTween.rotate (cam.gameObject, farPos.eulerAngles, 0.45f);
		}

	}
}
