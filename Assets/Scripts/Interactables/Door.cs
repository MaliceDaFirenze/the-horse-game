using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable {
	public GameObject doorGO;
	public Collider doorCollider;
	public Transform closedPosition;
	public Transform openPosition;

	private bool isOpen;
	private float openDuration = 0.3f;

	public override void PlayerInteracts(Player player){
		base.PlayerInteracts (player);

		if (isOpen) {
			isOpen = false;
			LeanTween.move (doorGO, closedPosition.position, openDuration);
			LeanTween.rotate (doorGO, closedPosition.eulerAngles, openDuration);
		} else {
			isOpen = true;
			LeanTween.move (doorGO, openPosition.position, openDuration);
			LeanTween.rotate (doorGO, openPosition.eulerAngles, openDuration);
		}
		doorCollider.enabled = false;
		StartCoroutine(ResetCollider(true));
	}

	private IEnumerator ResetCollider (bool setTo){
		yield return new WaitForSeconds (openDuration);
		doorCollider.enabled = setTo;
	}
}
