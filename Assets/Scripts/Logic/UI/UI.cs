using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	//UI Element Refs

	public GameObject[] instructionGOs;
	public Text[] instructionTexts;
	public Image[] instructionImages;

	public GameObject arrowSequenceGO;
	public Image[] arrows;

	public Text moneyText;

	//Inventory
	public GameObject inventoryUI;
	public Image activeSlotImage;
	public Image activeFrameImage;
	public Image[] slotImages;
	public Image[] frameImages;
	public Transform regularInvPos;
	public Transform dialogueInvPos;
	//

	//dialogue
	public GameObject dialogueBox;
	public Text dialogueText;
	public Image portraitImage;
	public GameObject rewardUIElements; //hide all if no reward
	public Image rewardIcon;
	public Text rewardText;
	//

	//Other Menues
	public ConstructionWindowUI constructionUI;
	public QuestUI questUI;
	//

	public Color arrowCompleteColor;
	public Color arrowIncompleteColor;
	private Vector3 arrowOriginalScale;
	private Vector3 arrowBigScale;

	public Color selectedActionOptionColor;
	public Color inactiveActionOptionColor;

	public HorseUI horseUI;
	public HorseRidingUI ridingUI;

	private Vector3 originalArrowsPos;

	public bool arrowCompletionFXInProgress;

	private Player lastRelevantPlayer;
	private Interactable lastRelevantInteractable;

	public bool dialogueIsVisible { get; private set; }
	private Reward currentReward = new Reward(RewardType.NONE, 0);


	private Dictionary <RewardType, Sprite> rewardIcons = new Dictionary<RewardType, Sprite>();

	void Start(){
		HideInstruction ();
		originalArrowsPos = arrowSequenceGO.transform.position;
		arrowOriginalScale = arrows[0].transform.localScale;
		arrowBigScale = arrowOriginalScale * 1.4f;
		dialogueBox.SetActive (false);

		//setup reward icons. do that with a for loop by type once I actually have several?
		rewardIcons.Add(RewardType.MONEY, Resources.Load<Sprite>("RewardIcons/icon-" + RewardType.MONEY));
	}

	public void ShowDialogue(string dialogue, Sprite portrait, Character id, Reward reward = new Reward()){

		Debug.Log ("show dialogue with reward: " + reward.rewardType + ", " + reward.rewardAmount);

		dialogueIsVisible = true;
		if (lastRelevantPlayer != null) {
			lastRelevantPlayer.allowPlayerInput = false;
		}

		dialogueBox.SetActive (true);
		portraitImage.sprite = portrait;
		inventoryUI.transform.position = dialogueInvPos.position;

		dialogueText.text = id.ToString() + ": \n" + dialogue;

		currentReward = reward;
		if (reward.rewardType == RewardType.NONE) {
			rewardUIElements.SetActive (false);
		} else {
			rewardUIElements.SetActive (true);
			rewardIcon.sprite = rewardIcons [reward.rewardType];

			//assign reward icon based on an icon dictionary?
			rewardText.text = reward.rewardAmount.ToString();
		}
	}

	public void ContinueInDialogue(){
		//eventually, this needs to handle multi-line dialogues
		//tie dialogues in sequences somehow? 

		//actually get reward upon continuing
		switch (currentReward.rewardType) {
		case RewardType.MONEY:
			PlayerEconomy.ReceiveMoney (currentReward.rewardAmount);
			break;
		default:
			break;
		}

		//if (this line is last in sequence){
		inventoryUI.transform.position = regularInvPos.position;
		dialogueBox.SetActive (false);
		if (lastRelevantPlayer != null) {
			lastRelevantPlayer.allowPlayerInput = true;
		}
		dialogueIsVisible = false;
		currentReward.rewardType = RewardType.NONE;
	}

	public void ShowInstruction(Interactable interactable, Player player){

		lastRelevantInteractable = interactable;
		lastRelevantPlayer = player;

		List<string> possibleActions = interactable.DefineInteraction (player);

		if (possibleActions.Count > 0) {

			if (interactable.selectedInteractionIndex >= interactable.currentlyRelevantActionIDs.Count) {
				//if three actions were possible and one no longer is, the selection should go to the 'second' action
				interactable.selectedInteractionIndex = interactable.currentlyRelevantActionIDs.Count - 1;
			}

			interactable.arrowInputRequired = ArrowSequences.GetArrowSequence (interactable.currentlyRelevantActionIDs [interactable.selectedInteractionIndex]);

			for (int i = 0; i < instructionGOs.Length; ++i) {
				if (i < possibleActions.Count) {
					instructionGOs [i].SetActive (true);
					if (i == interactable.selectedInteractionIndex) {
						instructionImages [i].color = selectedActionOptionColor;
					} else {
						instructionImages [i].color = inactiveActionOptionColor;
					}

					if (ArrowSequences.GetArrowSequence (interactable.currentlyRelevantActionIDs [i]) != null) {
						instructionTexts [i].text = interactable.DefineInteraction (player) [i];
					} else {
						instructionTexts [i].text = "E - " + interactable.DefineInteraction (player) [i];
					}
				} else {
					instructionGOs [i].SetActive (false);
				}
			}

			//--Debug--//
			if (interactable.currentlyRelevantActionIDs.Count != possibleActions.Count) {
				Debug.LogError ("different count for possible actions (" + possibleActions.Count + ") and currentlyRelevantActionIDs (" + interactable.currentlyRelevantActionIDs.Count + ") on interactable: " + interactable);
				string log = "possible actions: ";
				for (int i = 0; i < possibleActions.Count; ++i) {
					log += "\n" + possibleActions [i];
				}
				Debug.LogWarning (log);
			}
			//----//

			if (interactable.arrowInputRequired != null) {
				for (int i = 0; i < arrows.Length; ++i) {
					if (i < interactable.arrowInputRequired.Length) {
						arrows [i].rectTransform.eulerAngles = new Vector3 (0, 0, 90 * (int)interactable.arrowInputRequired [i]); 
						arrows [i].enabled = true;
					} else {
						arrows [i].enabled = false;
					}
				}
				arrowSequenceGO.SetActive (true);
				UpdateArrows (0);
			} else {
				arrowSequenceGO.SetActive (false);
			}
		} else {
			HideInstruction ();
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
		arrowCompletionFXInProgress = true;
		for (int i = 0; i < arrows.Length; ++i) {
			if (arrows [i].gameObject.activeSelf) {
				LeanTween.scale (arrows [i].gameObject, arrowBigScale, 0.14f);
			}
		} 
		yield return new WaitForSeconds (0.14f);
		for (int i = 0; i < arrows.Length; ++i) {
			if (arrows [i].gameObject.activeSelf) {
				LeanTween.scale (arrows [i].gameObject, arrowOriginalScale, 0.14f);
			}
		} 

		yield return new WaitForSeconds (0.16f);

		for (int i = 0; i < arrows.Length; ++i) {
			arrows [i].transform.localScale = arrowOriginalScale;
		} 

		ShowInstruction (lastRelevantInteractable, lastRelevantPlayer);
		arrowCompletionFXInProgress = false;
	}

	public void HideInstruction(){

		for (int i = 0; i < instructionGOs.Length; ++i) {
			instructionGOs[i].SetActive (false);
		}
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

	//---Singleton---//
	private static UI _instance;
	public static UI instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<UI> ();
				if (_instance == null) {
					Debug.LogWarning ("no UI found");
				}
				//DontDestroyOnLoad (_instance.gameObject);
			} 
			return _instance;
		}
	}
}
