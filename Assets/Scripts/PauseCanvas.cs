using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;

public class PauseCanvas : MonoBehaviour {

	private GameObject panel;

	private bool showMenu;

	// Use this for initialization
	void Start () {
		panel = transform.GetChild(0).gameObject;
		showMenu = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			flipMenu();		
		}

		if(!showMenu && panel.activeSelf){
			panel.SetActive(false);
		}
	}

	void flipMenu(){

		showMenu = !showMenu;

		if(showMenu){
			panel.SetActive(true);
			Time.timeScale = 0;
		} else{
			Time.timeScale = 1;
		}
	}

	public void Continue(){
		flipMenu();
	}

	public void RestartLevel(){
		Time.timeScale = 1;
		Application.LoadLevel (Application.loadedLevel);
		//SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	public void Quit(){
		Application.Quit();
	}
}
