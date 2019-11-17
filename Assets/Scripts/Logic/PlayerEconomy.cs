using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEconomy : MonoBehaviour {

	//store the money int value
	//display the value or better: tell some UI script to do that
	//getter / setter for money value
	//buy and sell functions

	[SerializeField]
	private static int money;
	public static int Money{
		get {
			return money;
		}
		private set { 
			money = value;
			MoneyValueWasUpdated ();
		}
	}

	public static void PayMoney(int amount){
		Money -= amount;
	}

	public static void ReceiveMoney(int amount){
		Money += amount;
	}

	public static void LoadMoneyFromSave(int newValue){
		//Debug.Log ("loaded money from save: " + newValue);
		Money = newValue;
	}

	private static void MoneyValueWasUpdated(){
		UI.instance.moneyText.text = "¢ " + Money.ToString();
	}
}
