using UnityEngine;
using System.Collections;

public class HoverShow : MonoBehaviour {

	public Renderer[] renderers;

	private Ray ray;
	private RaycastHit hit;

	// Update is called once per frame
	void Start () {
		hide ();
	}

	void Update(){
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {

			if (hit.collider.gameObject.Equals (this.gameObject))
				show ();
			else
				hide ();
		} else {
			hide();
		}
	}

	void show(){
		for (int i = 0; i < renderers.Length; i++) {
			renderers[i].enabled = true;
		}
	}


	void hide(){
		for (int i = 0; i < renderers.Length; i++) {
			renderers[i].enabled = false;
		}
	}
}
