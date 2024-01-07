using UnityEngine;
using System.Collections;

public class PoliceStation : MonoBehaviour {

	public float safeRadius;
	public float mediumRadius;

	private GameObject safeSphere;
	private GameObject mediumSphere;
	public float safeSmallNum;
	public float safeMediumNum;
	// Use this for initialization
	void Start () {
		safeSphere = transform.FindChild ("DebugSphere_safe").gameObject;
		safeSphere.transform.localScale = new Vector3 (safeRadius*2/transform.localScale.x, safeSmallNum, safeRadius*2/transform.localScale.z);

		mediumSphere = transform.FindChild ("DebugSphere_medium").gameObject;
		mediumSphere.transform.localScale = new Vector3 (mediumRadius*2/transform.localScale.x,safeMediumNum, mediumRadius*2/transform.localScale.z);
	}

}
