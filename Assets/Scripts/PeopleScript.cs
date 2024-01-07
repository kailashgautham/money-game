using UnityEngine;
using System.Collections;

public class PeopleScript : MonoBehaviour {

	private float radius;
	private Vector3 origin;

	private UnityEngine.AI.NavMeshAgent agent;
	private bool onMyWay;
	private Vector3 target;

	public void setOriginRadius(Vector3 or, float r){
		origin = or;
		radius = r;
	}

	void Start () {
		agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ();

	}
	

	void Update () {



		if (!onMyWay) {
			target = getRandomPosition ();
			onMyWay = true;
			agent.SetDestination (target);
		} else if (onMyWay && agent.remainingDistance <= 1) {
			onMyWay = false;
		}
	
	}

	Vector3 getRandomPosition(){
		Vector2 randomAbsV2 = Random.insideUnitCircle * radius;
		Vector3 position = new Vector3(randomAbsV2.x, 0, randomAbsV2.y) + (origin);
		return position;
	}
}
