using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEconomy : MonoBehaviour {

	//store the money int value
	//display the value or better: tell some UI script to do that
	//getter / setter for money value
	//buy and sell functions

	[SerializeField]
	private int money;
	public int Money{
		get {
			return money;
		}
		private set { 
			money = value;
			MoneyValueWasUpdated ();
		}
	}

	public void LoadMoneyFromSave(int newValue){
		Debug.Log ("loaded money from save: " + newValue);
		Money = newValue;
	}

	private void MoneyValueWasUpdated(){
		ui.moneyText.text = "¢ " + Money.ToString();
	}

	private UI _ui;
	private UI ui{
		get { 
			if (_ui == null) {
				_ui = FindObjectOfType<UI> ();
			}
			return _ui;
		}
	}

}
