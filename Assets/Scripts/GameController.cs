using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public float levelOneAmount;
	public float levelTwoAmount;
	public float levelThreeAmount;
	public Text moneyBarText;
	public  float targetMoney;
	public float shopDistance;
	public float totalMoney;
	public Text targetText;
	public float moneyLost;
	public int shopsLost;
	public int robTolerance;

	// Use this for initialization
	void Start()
	{

		targetText.text = "Target: Rs. " + targetMoney + " K";
	}
	
	// Update is called once per frame
	void Update () {

		moneyBarText.text = "Rs. " +   totalMoney + " K";
		if (totalMoney >= targetMoney)
			Application.LoadLevel (Application.loadedLevel + 1);
	}

	public void addMoney(float m){
		totalMoney += m;
	}

	public void removeMoney(){
	}

	public bool canBuildLevel1(){

			
			return levelOneAmount <= totalMoney;

	}

	public bool canBuildLevel2(){
		return levelTwoAmount <= totalMoney;
	}

	public bool canBuildLevel3(){
		return levelThreeAmount <= totalMoney;
	}

	public void builtLevel(int lvl){
		float amnt = 0;
		if (lvl == 1)
			amnt = levelOneAmount;
		else if(lvl == 2)
			amnt = levelTwoAmount;
		else if(lvl == 3)
			amnt = levelThreeAmount;
	
		totalMoney -= amnt;
	}
}


