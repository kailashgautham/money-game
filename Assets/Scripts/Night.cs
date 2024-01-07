using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Night : MonoBehaviour {
	public float fadeDuration;
	public float dayDuration;
	private Image image;
	private GameObject[] shops;
	public Image notifyImage;
	public float notifyDuration;
	public Text shopsLostText;
	public Text moneyLostText;
	private GameController gc;
	public GameObject panel;
	public Vector3 panelPosition;
	// Use this for initialization
	void Start () {
		panelPosition = new Vector3 (panel.transform.position.x,panel.transform.position.y,panel.transform.position.z);
		gc = GameObject.Find("GameController").GetComponent<GameController> ();
		image = gameObject.GetComponent<Image> ();
		image.canvasRenderer.SetAlpha(0.1f);
		notifyImage.canvasRenderer.SetAlpha (0.001f);
		shopsLostText.canvasRenderer.SetAlpha (0.001f);
		moneyLostText.canvasRenderer.SetAlpha (0.001f);
		//image.CrossFadeAlpha(255f, fadeDuration, false);

	}
	
	// Update is called once per frame
	private bool dayIsOn;
	private float dayStartTime;
	void Update () {

		//its night time now
		if (image.canvasRenderer.GetAlpha() == 255f) {
			shops = GameObject.FindGameObjectsWithTag ("Shop");
			for(int i = 0; i < shops.Length; i++)
				shops[i].SendMessage("nightTimeCame");
			//image.canvasRenderer.SetAlpha(255f);
			image.CrossFadeAlpha(0.01f, fadeDuration/3, false);

			notifyImage.CrossFadeAlpha(255f,fadeDuration/4,false);
			shopsLostText.CrossFadeAlpha(255f,fadeDuration/4,false);
			moneyLostText.CrossFadeAlpha(255f,fadeDuration/4,false);

			setRobText();
			gc.shopsLost = 0;
			gc.moneyLost = 0.0f;

			this.GetComponent<AudioSource>().Play();
		} 

		//its daylight now
		if (image.canvasRenderer.GetAlpha() <= 0.1f) {
			if(!dayIsOn){

				if (panel.transform.position.y != panelPosition.y)
					panel.transform.Translate (0,135,0);
				dayIsOn = true;
				dayStartTime = Time.time;
				//notifyImage.CrossFadeAlpha(0.1f,dayDuration/2,false);
			}else if(Time.time - dayStartTime >= dayDuration){
				notifyImage.canvasRenderer.SetAlpha (0.001f);
				shopsLostText.canvasRenderer.SetAlpha (0.001f);
				moneyLostText.canvasRenderer.SetAlpha (0.001f);
				image.CrossFadeAlpha(255f, fadeDuration, false);
				dayIsOn = false;
				panel.transform.Translate (0,-135,0);


			}
		}
	}

	void setRobText(){
		if (gc.shopsLost == 0)
		{
			Debug.Log ("No. of shops lost " + gc.shopsLost);
			shopsLostText.text = "All Your Shops Are Safe!";
			moneyLostText.text = "No money lost!";
		}
		else if (gc.shopsLost == 1)
		{
			shopsLostText.text = gc.shopsLost + " Shop Was Robbed.";
			moneyLostText.text = "You Lost Rs." + gc.moneyLost + " K.";
		}
		else{
			shopsLostText.text = gc.shopsLost + " Shops Were Robbed.";
			moneyLostText.text = "You Lost Rs." + gc.moneyLost + " K.";
		}
	}
}
