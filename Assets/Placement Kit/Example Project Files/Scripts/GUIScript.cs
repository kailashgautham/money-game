using UnityEngine;
using UnityEngine.UI;


public class GUIScript : MonoBehaviour {

    BuildManager bm;
	public Button shopL1, shopL2, shopL3;
	GameController gc;
	Color newColor;

    // Use this for initialization
    void Start () {

		gc = GameObject.Find("GameController").GetComponent<GameController> ();
		bm = GameObject.Find("BuildManager").GetComponent<BuildManager>();
       

		shopL1.GetComponentInChildren<Text> ().text = "Cost: Rs. " + gc.levelOneAmount + " K";

		shopL2.GetComponentInChildren<Text> ().text = "Cost: Rs. " + gc.levelTwoAmount + " K";
	
		shopL3.GetComponentInChildren<Text> ().text = "Cost: Rs. " + gc.levelThreeAmount + " K";
	
		shopL1.targetGraphic.CrossFadeAlpha (0.3f, 0.0f, true);
		shopL2.targetGraphic.CrossFadeAlpha (0.3f, 0.0f, true);
		shopL3.targetGraphic.CrossFadeAlpha (0.3f, 0.0f, true);


    }

    public void ActiveteBuilding()
    {
		if (gc.canBuildLevel1 ()) {
			shopL1.gameObject.GetComponent<Button> ().interactable = true;
			shopL1.targetGraphic.CrossFadeAlpha(1f,1f,false);
		} else {
			shopL1.gameObject.GetComponent<Button> ().interactable = false;
			shopL1.targetGraphic.CrossFadeAlpha (0.3f, 0f, true);
		}

		if (gc.canBuildLevel2 ()) {
			shopL2.gameObject.GetComponent<Button> ().interactable = true;
			shopL2.targetGraphic.CrossFadeAlpha (1f,1f,false);
		} else {
			shopL2.gameObject.GetComponent<Button> ().interactable = false;
			shopL2.targetGraphic.CrossFadeAlpha (0.3f, 0f, true);
		}

		if (gc.canBuildLevel3 ()) 
		{
			shopL3.gameObject.GetComponent<Button> ().interactable = true;
			shopL3.targetGraphic.CrossFadeAlpha (1f,1f,false);
		} else {
			shopL3.gameObject.GetComponent<Button> ().interactable = false;
			shopL3.targetGraphic.CrossFadeAlpha (0.3f, 0.0f, true);
		}
               
              
            
     }
        
        
	public void buildLevel(int lvl){
		bm.SelectBuilding(lvl-1);
		bm.ActivateBuildingmode();
	}
           

    

    void Update()
    {
		ActiveteBuilding ();
            if (!bm.isBuildingEnabled)
            {
                if (shopL1.image.color != Color.white)
					shopL1.image.color = Color.white;

				if (shopL2.image.color != Color.white)
					shopL2.image.color = Color.white;

				if (shopL3.image.color != Color.white)
					shopL3.image.color = Color.white;
			}
        }
    }

