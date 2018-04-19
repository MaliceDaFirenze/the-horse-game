using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	public GameObject instructionGO;
	public Text instructionText;

	public GameObject arrowSequenceGO;
	public Image[] arrows;

	public Color arrowCompleteColor;
	public Color arrowIncompleteColor;
	private Vector3 arrowOriginalScale;
	private Vector3 arrowBigScale;

	public HorseUI horseUI;

	private Vector3 originalArrowsPos;

	void Start(){
		HideInstruction ();
		originalArrowsPos = arrowSequenceGO.transform.position;
		arrowOriginalScale = arrowSequenceGO.transform.localScale;
		arrowBigScale = arrowOriginalScale * 1.1f;
	}

	public void ShowInstruction(Interactable interactable, Player player){
		if (interactable.GetInteractionString (player) != "") {
			instructionGO.SetActive (true);
			instructionText.text = "E - " + interactable.GetInteractionString(player);

			interactable.arrowInputRequired = ArrowSequences.GetArrowSequence (interactable.currentlyRelevantActionID);
			if (interactable.arrowInputRequired != null) {
				for (int i = 0; i < arrows.Length; ++i) {
					if (i < interactable.arrowInputRequired.Length) {
						arrows[i].rectTransform.eulerAngles = new Vector3(0,0, 90 * (int)interactable.arrowInputRequired[i]); 
						arrows [i].enabled = true;
					} else {
						arrows [i].enabled = false;
					}
				}
				arrowSequenceGO.SetActive (true);
				UpdateArrows (0);
			}
		}
	}

	public void UpdateArrows(int completeUntilIndex){
		for (int i = 0; i < arrows.Length; ++i) {
			if (i < completeUntilIndex) {
				arrows [i].color = arrowCompleteColor;
			} else {
				arrows [i].color = arrowIncompleteColor;
			}
		}
	}

	public void ArrowSequenceComplete(){
		StartCoroutine(ArrowSequenceCompleteEffects());
	}

	private IEnumerator ArrowSequenceCompleteEffects(){
		for (int i = 0; i < arrows.Length; ++i) {
			if (arrows [i].gameObject.activeSelf) {
				LeanTween.scale (arrows [i].gameObject, arrowBigScale, 0.15f);
			}
		} 
		yield return new WaitForSeconds (0.15f);
		for (int i = 0; i < arrows.Length; ++i) {
			if (arrows [i].gameObject.activeSelf) {
				LeanTween.scale (arrows [i].gameObject, arrowOriginalScale, 0.1f);
			}
		} 
		yield return new WaitForSeconds (0.1f);

		for (int i = 0; i < arrows.Length; ++i) {
			arrows [i].transform.localScale = arrowOriginalScale;
		} 
		HideInstruction();
	
	}

	public void HideInstruction(){
		instructionGO.SetActive (false);
		arrowSequenceGO.SetActive (false);
	}

	public void ShowHorseUI (Horse_Stats horse){
		horseUI.ShowUIForHorse (horse);
	}

	public void HideHorseUI(){
		horseUI.Hide ();
	}


	private float shakeDuration;// = 0f;
	private float shakeAmount;// = 0.5f;
	private float decreaseFactor = 1.0f;
	private WaitForSeconds shakeWait = new WaitForSeconds(0.075f);

	public void ShakeArrows(float duration = 0.1f, float amount = 8f){
		Debug.Log ("shake dur: " + duration + ", amount: " + amount);
		shakeAmount = amount;
		shakeDuration = duration;

		StartCoroutine (ArrowShake ());
	}

	private IEnumerator ArrowShake()	{
		while (shakeDuration > 0) {
		//	arrowSequenceGO.transform.position = arrowSequenceGO.transform.position + Random.insideUnitSphere * shakeAmount;
			arrowSequenceGO.transform.position = new Vector3(arrowSequenceGO.transform.position.x + shakeAmount, arrowSequenceGO.transform.position.y, arrowSequenceGO.transform.position.z);
			shakeAmount *= -0.9f;
			shakeDuration -= Time.deltaTime * decreaseFactor;
			yield return shakeWait;
		}

		shakeDuration = 0f;
		arrowSequenceGO.transform.position = originalArrowsPos;
	}
}
