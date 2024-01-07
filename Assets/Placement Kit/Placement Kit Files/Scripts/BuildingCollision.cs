using UnityEngine;
using System.Collections;

/// <summary>
/// Building collision.
/// 
/// This script is attached to the buildings.
/// </summary>

public class BuildingCollision : MonoBehaviour {
	private GameObject[] shops;
    private bool isCollided = false;
	private BuildManager buildMan = null;
	private GameController gc;
	private bool stayed;
	public bool Collided()
    {
		if (buildMan == null)
			return true;

		shops = GameObject.FindGameObjectsWithTag ("Shop");
		for (int i = 0; i<shops.Length; i++)
		{


				if (shops[i].name.Contains ("Ghost"))
				    continue;

			if (Vector3.Distance (buildMan.ghost.transform.position,shops[i].transform.position) < gc.shopDistance)
				{

					return true;
			}

		}
        return isCollided;
    }

    

    void Start()
    {
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
        buildMan = GameObject.Find("BuildManager").GetComponent<BuildManager>();

    }

	void OnCollisionEnter(Collision collision)
	{
		isCollided = true;
	}
	void OnCollisionStay(Collision collision)
	{
		if (collision.collider.gameObject.tag != buildMan.TerrainCollisionTag)
		{
			isCollided = true;
            stayed = true;
			Debug.Log ("Collided" + collision.collider.gameObject.name);
		}
	}

	void OnCollisionExit()
	{
		if (stayed) {
			Debug.Log ("Gone");
			isCollided = false;
			stayed = false;
		}
	}

	void OnTriggerEnter(Collision collision)
	{
		isCollided = true;
	}

	void OnTriggerStay(Collider collision)
	{
		if (collision.gameObject.tag != buildMan.TerrainCollisionTag)
		{
			stayed = true;
			isCollided = true;
			Debug.Log ("Collided" + collision.gameObject.name);
		}
	}
	
	void OnTriggerExit()
	{
		if (stayed) {
			Debug.Log ("Gone");
			isCollided = false;
			stayed = false;
		}
	}



}
