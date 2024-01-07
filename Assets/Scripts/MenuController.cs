using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {


	public void playGame()
	{
		Application.LoadLevel (1);
	}

	public void Quit()
	{
		Application.Quit ();
	}

}

