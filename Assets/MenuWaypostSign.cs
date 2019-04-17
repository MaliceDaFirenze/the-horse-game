using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MenuWaypostSign : MonoBehaviour {
	MenuWaypost menu;
	private MeshRenderer meshRenderer;
	public int signIndex;

	private Vector3 regularScale;
	private Vector3 activeScale;

	public Material[] regularMats;
	public Material[] activeMats;

	private void Start(){
		regularScale = transform.localScale;
		activeScale = regularScale * 1.1f;
		menu = transform.parent.GetComponent<MenuWaypost> ();
		meshRenderer = GetComponent<MeshRenderer> ();
	}

	private void OnMouseDown (){
		menu.ClickSign (signIndex);
		meshRenderer.materials = activeMats;
		StartCoroutine (ReturnToRegular ());
	}

	private IEnumerator ReturnToRegular(){
		yield return new WaitForSeconds (0.05f);
		meshRenderer.materials = regularMats;

	}

	private void OnMouseEnter (){
		LeanTween.scale (gameObject, activeScale, 0.07f);
	}

	private void OnMouseExit (){
		LeanTween.scale (gameObject, regularScale, 0.07f);
	}
}
