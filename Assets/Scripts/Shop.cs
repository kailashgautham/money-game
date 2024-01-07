using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Shop : MonoBehaviour {

	public float incomePerPerson;
	public float checkFrequency;
	public float radius;
	public Text moneyText;
	public int shopLevel;
	public GameObject rupee;

	//0 = safe
	//1 = medium
	//3 = risky
	private int riskLevel;
	private float personalMoney;
	private GameObject sphere;
	private float lastCheckTime;
	private GameController gc;
	private GameObject sceneCamera;
	private float distance;
	private GameObject[] policeStations;
	private bool robbed;
	public float smallY;

	private int numberOfTurns;
	private int numberOfRobbedTurns;

	// Use this for initialization
	void Start () {

		if (Application.loadedLevelName.Equals ("pranav_2"))
			transform.Rotate (new Vector3 (0, 0, 0));
		else
			transform.Rotate (new Vector3 (0, 90, 0));

		if (gameObject.name.Contains ("Ghost")) {
			//Debug.Log("ghost detected");
			moneyText.transform.parent.parent.gameObject.SetActive(false);
			this.enabled = false;
			return;
		}

		gameObject.GetComponent<AudioSource>().Play();
		gameObject.GetComponent<Collider> ().isTrigger = true;
		sceneCamera = GameObject.Find ("Main Camera");
		moneyText.text = "0 Rs.";
		sphere = transform.FindChild ("DebugSphere").gameObject;
		sphere.transform.localScale = new Vector3 (radius*2/transform.localScale.x, smallY, radius*2/transform.localScale.z);
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();

		policeStations = GameObject.FindGameObjectsWithTag ("PoliceStation");
		int bestLevel = 3;
		for (int i = 0; i < policeStations.Length; i++) {

			distance = Vector3.Distance (transform.position, policeStations[i].transform.position);
			if (distance <=policeStations[i].GetComponent<PoliceStation>().safeRadius)
			{
				riskLevel = 0;
			}
			else if (distance <=policeStations[i].GetComponent<PoliceStation>().mediumRadius)
			{
				riskLevel = 1;
			}
			else
			{
				riskLevel = 3;
			}

			if(i == 0)
				bestLevel = riskLevel;
			else if(riskLevel < bestLevel)
				bestLevel = riskLevel;
		}
		riskLevel = bestLevel;
		setUIRiskColor (riskLevel);
		gc.builtLevel (shopLevel);
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.GetChild(0).transform.LookAt (sceneCamera.transform);
		gameObject.transform.GetChild(0).Rotate(0,180,0);

		if (robbed) {
			return;
		}

		gameObject.transform.GetChild(0).transform.LookAt (sceneCamera.transform);
		gameObject.transform.GetChild(0).Rotate(0,180,0);

		if (Time.time - lastCheckTime >= checkFrequency) {
			Collider[] inRadius = Physics.OverlapSphere (transform.position, radius, ~(1 << 8));
			personalMoney+= inRadius.Length * incomePerPerson;
			updateUIMoney(inRadius.Length * incomePerPerson);
			gc.addMoney(inRadius.Length * incomePerPerson);
			lastCheckTime = Time.time;
		}
	}

	void updateUIMoney(float amount){
		moneyText.text ="Rs." +  amount + " K";
		if (amount != 0)
			GameObject.Instantiate (rupee, transform.position,Quaternion.identity);
		//UI work
	}

	void setUIRiskColor(int level){
		switch (level)
		{
		case 0: 
			moneyText.GetComponentInParent<Image>().color = Color.green;
			break;
		case 1: 
			moneyText.GetComponentInParent<Image>().color = Color.yellow;
			break;
		case 3: 
			moneyText.GetComponentInParent<Image>().color = Color.red;
			break;
		}
	}

	public void nightTimeCame(){

		if(gc == null)
			gc = GameObject.Find ("GameController").GetComponent<GameController> ();

		numberOfTurns++;

		if (robbed) {
			numberOfRobbedTurns++;
			if(numberOfRobbedTurns == 1)
				GameObject.DestroyImmediate(gameObject);
			return;
		}

		bool increaseRobProb = numberOfTurns >= gc.robTolerance;

		if (riskLevel == 0)
			return;

		if (riskLevel == 1) {
			int num = Random.Range(0, 3);
			if(num == 0)
				shopRobbed();
			else if(increaseRobProb){
				num = Random.Range(0, 3);
				if(num == 0)
					shopRobbed();
			}
		}

		if (riskLevel == 3) {
			int num = Random.Range(0, 2);
			if(num == 0)
				shopRobbed();
			else if(increaseRobProb){
				num = Random.Range(0, 2);
				if(num == 0)
					shopRobbed();
			}
		}
	}

	void shopRobbed(){
		gc.shopsLost += 1;
		robbed = true;
		moneyText.text = "Robbed!";
		moneyText.color = Color.white;
		moneyText.transform.parent.GetComponent<Image>().color = Color.black;
		if (shopLevel == 1) {
			gc.moneyLost += gc.levelOneAmount / 2;
			gc.addMoney (-1 * gc.levelOneAmount / 2);
		} else if (shopLevel == 2) {
			gc.moneyLost += gc.levelTwoAmount / 2;
			gc.addMoney (-1 * gc.levelTwoAmount / 2);
		} else if (shopLevel == 3) {
			gc.moneyLost += gc.levelThreeAmount / 2;
			gc.addMoney (-1 * gc.levelThreeAmount / 2);
		}

		Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer> ();
		for (int i = 0; i < renderers.Length; i++)
			renderers [i].material.color = Color.grey;
	}
}