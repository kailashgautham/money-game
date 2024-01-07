using UnityEngine;
using System.Collections;

public class HouseScript : MonoBehaviour {
	public Material[] materials;
	public int population;
	public float radius;
	public GameObject people;
	private GameObject sphere;
	public float smallY;
	// Use this for initialization
	void Start () {
		sphere = transform.FindChild ("DebugSphere").gameObject;
		sphere.transform.localScale = new Vector3 (radius*2/transform.localScale.x, smallY, radius*2/transform.localScale.z);
		for (int i = 0; i < population; i++)
			generatePerson ();
	}


	void generatePerson(){
		Vector2 randomAbsV2 = Random.insideUnitCircle * radius;
		Vector3 position = new Vector3(randomAbsV2.x, 0, randomAbsV2.y) + (gameObject.transform.position);
		GameObject person = (GameObject)Instantiate (people, position, Quaternion.identity);
		person.GetComponent<PeopleScript> ().setOriginRadius (transform.position, radius);
		person.GetComponentInChildren<SkinnedMeshRenderer> ().material = getRandomMat ();
	}

	Material getRandomMat(){
		return materials[Random.Range (0,materials.Length)];
	}
}
