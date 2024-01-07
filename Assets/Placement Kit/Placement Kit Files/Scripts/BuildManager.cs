using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Build manager.
/// 
/// This Script is attached to a empty GameObject
/// </summary>

[System.Serializable]
public class Snapping
{
    public bool snappingEnabled = false;
    public float snapRadius = 2.0f;
    public float snapMagin = 0.0f;
    public SnapSides snapSides;
}

[System.Serializable]
public struct SnapSides
{
    // based on objects facing direction
    public bool left, right, top, bottom, front, back;
}

public class BuildManager: MonoBehaviour 
{
	public int SelectedBuilding;
	private int LastSelectedBuilding;
	public GameObject[] Building;
	
	public List<BuildingList> buildings = new List<BuildingList>();
	
	public string TerrainCollisionTag;

    private bool ghostOn = false;
    public GameObject ghost;
    private BuildingCollision ghostCollision;

	private bool isFlat;
	public float maxSlopeHigh = 5f;

    public bool isBuildingEnabled { get; private set; }

    // Unused until fully implemented
   // public Snapping snapping = new Snapping();

    void Start()
	{
		LastSelectedBuilding = SelectedBuilding;
        isBuildingEnabled = false;

    }

    public void ActivateBuildingmode()
    {
        isBuildingEnabled = true;
    }

    public void DeactivateBuildingmode()
    {
        isBuildingEnabled = false;
    }

    public void SelectBuilding(int id)
    {
        if (id < Building.Length && id >= 0)
        {
            LastSelectedBuilding = SelectedBuilding;
            SelectedBuilding = id;
        }
    }

    void Update()
	{
        if (!isBuildingEnabled)
        {
            if (ghost != null)
            {
                Destroy(ghost);
                ghostOn = false;
            }
                
            return;
        }
            

		Ray ray;
		RaycastHit[] hit;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		hit = Physics.RaycastAll(ray, Mathf.Infinity);
	
		for (int i = 0; i < hit.Length; i++)
		{
            if (hit[i].collider.tag == TerrainCollisionTag)
			{
                

                if (SelectedBuilding != LastSelectedBuilding && ghost != null)
				{
					Destroy(ghost);
					ghostOn = false;
					LastSelectedBuilding = SelectedBuilding;
					
					break;
				}
				
				if (!ghostOn)
				{
					ghost = (GameObject)Instantiate(Building[SelectedBuilding], 
					new Vector3(hit[i].point.x,
						hit[i].point.y, 
						hit[i].point.z), 
						Quaternion.identity);

                    ghost.name = ghost.name.Replace("(Clone)", "(Ghost)");
                    ghost.layer = 2; //ignore raycast layer

                    ghostCollision = ghost.AddComponent<BuildingCollision>();
		
					ghostOn = true;	
				}
	
				if (ghost != null)
				{
					if (Input.GetMouseButtonDown(0) && !ghostCollision.Collided() && isFlat)
                    {
                        BuildingList bl = new BuildingList();

                        DestroyImmediate(ghost);

                        bl.buildingGameObject = (GameObject)Instantiate(Building[SelectedBuilding],
                        new Vector3(hit[i].point.x,
                            hit[i].point.y + Building[SelectedBuilding].GetComponent<Collider>().transform.localScale.y / 2,
                            hit[i].point.z),
                        Quaternion.identity);

                        string s = bl.buildingGameObject.name.Replace("(Clone)", "");
                        bl.buildingGameObject.name = s;
                        bl.buildingName = s;
                        bl.buildingGameObject.AddComponent<BuildingCollision>();

                        buildings.Add(bl);

                        ghostOn = false;

                        DeactivateBuildingmode();

                        break;
                    }

                    Vector3 ghostTargetPos = new Vector3(
                        hit[i].point.x,
                        hit[i].point.y + Building[SelectedBuilding].GetComponent<Collider>().transform.localScale.y / 2,
                        hit[i].point.z);

                    ghost.transform.position = ghostTargetPos;

					isFlat = GroundFlat(ghost);
						
					if (ghostCollision.Collided() || !isFlat)
					{
						//ghost.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(Building[SelectedBuilding].GetComponent<Renderer>().sharedMaterial);
						Renderer[] renderers = ghost.GetComponentsInChildren<Renderer>();
						for(int k = 0; k < renderers.Length; k++){
							renderers[k].material.color = new Color(
								1f,
								0f, 
								0f, 
								0.6f);
						}
					}			
					else if (!ghostCollision.Collided() && isFlat)
					{
						//ghost.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(Building[SelectedBuilding].GetComponent<Renderer>().sharedMaterial);
						Renderer[] renderers = ghost.GetComponentsInChildren<Renderer>();
						for(int k = 0; k < renderers.Length; k++){
							renderers[k].material.color = new Color(
							0f,
							1f, 
							0f, 
							0.6f);
						}
					}
			
				}
			}	
		}

		if (ghost != null && Input.GetMouseButtonDown(1))
		{
			GameObject.Destroy(ghost);
			ghostOn = false;
			DeactivateBuildingmode();
		}
	}

	private bool GroundFlat(GameObject Ghost)
	{
		//return true;
		RaycastHit[] buildingSlopeHitUL;
		RaycastHit[] buildingSlopeHitUR;
		RaycastHit[] buildingSlopeHitDL;
		RaycastHit[] buildingSlopeHitDR;
		RaycastHit[] buildingSlopeHitM;

        buildingSlopeHitUL = Physics.RaycastAll(new Vector3(
			ghost.GetComponent<Collider>().transform.position.x - ghost.transform.localScale.x / 2,
			ghost.GetComponent<Collider>().transform.position.y + Building[SelectedBuilding].GetComponent<Collider>().transform.localScale.y / 2,
			ghost.GetComponent<Collider>().transform.position.z - ghost.transform.localScale.z / 2),
			Vector3.down, Mathf.Infinity);

        buildingSlopeHitUR = Physics.RaycastAll(new Vector3(
			ghost.GetComponent<Collider>().transform.position.x + ghost.transform.localScale.x / 2,
			ghost.GetComponent<Collider>().transform.position.y + Building[SelectedBuilding].GetComponent<Collider>().transform.localScale.y / 2,
			ghost.GetComponent<Collider>().transform.position.z - ghost.transform.localScale.z / 2),
			Vector3.down, Mathf.Infinity);

        buildingSlopeHitDL = Physics.RaycastAll(new Vector3(
			ghost.GetComponent<Collider>().transform.position.x - ghost.transform.localScale.x / 2,
			ghost.GetComponent<Collider>().transform.position.y + Building[SelectedBuilding].GetComponent<Collider>().transform.localScale.y / 2,
			ghost.GetComponent<Collider>().transform.position.z + ghost.transform.localScale.z / 2),
			Vector3.down, Mathf.Infinity);

        buildingSlopeHitDR = Physics.RaycastAll(new Vector3(
			ghost.GetComponent<Collider>().transform.position.x + ghost.transform.localScale.x / 2,
			ghost.GetComponent<Collider>().transform.position.y + Building[SelectedBuilding].GetComponent<Collider>().transform.localScale.y / 2,
			ghost.GetComponent<Collider>().transform.position.z + ghost.transform.localScale.z / 2),
			Vector3.down, Mathf.Infinity);

        buildingSlopeHitM = Physics.RaycastAll(new Vector3(
			ghost.GetComponent<Collider>().transform.position.x,
			ghost.GetComponent<Collider>().transform.position.y + Building[SelectedBuilding].GetComponent<Collider>().transform.localScale.y / 2,
			ghost.GetComponent<Collider>().transform.position.z),
			Vector3.down, Mathf.Infinity);

		//if(buildingSlopeHitUL.Length != 0 || buildingSlopeHitUR.Length != 0 || buildingSlopeHitDL.Length != 0 || buildingSlopeHitDR.Length != 0 || buildingSlopeHitM.Length != 0)
		//if(buildingSlopeHitUL.Length == 0 || buildingSlopeHitUR.Length == 0 || buildingSlopeHitDL.Length == 0 || buildingSlopeHitDR.Length == 0 || buildingSlopeHitM.Length == 0)
		//	Debug.Log(buildingSlopeHitUL [0].collider.name);
        if ((buildingSlopeHitUL.Length > 0 ? (buildingSlopeHitUL [0].collider.tag == TerrainCollisionTag) : false) &&
			(buildingSlopeHitUR.Length > 0 ? (buildingSlopeHitUR [0].collider.tag == TerrainCollisionTag) : false) &&
			(buildingSlopeHitDL.Length > 0 ? (buildingSlopeHitDL [0].collider.tag == TerrainCollisionTag) : false) &&
			(buildingSlopeHitDR.Length > 0 ? (buildingSlopeHitDR [0].collider.tag == TerrainCollisionTag) : false) &&
			(buildingSlopeHitM.Length > 0 ? (buildingSlopeHitM [0].collider.tag == TerrainCollisionTag) : false)) {
			if (HitDistanceSmallerEqual (buildingSlopeHitUL [0], maxSlopeHigh) &&
				HitDistanceSmallerEqual (buildingSlopeHitUR [0], maxSlopeHigh) &&
				HitDistanceSmallerEqual (buildingSlopeHitDL [0], maxSlopeHigh) &&
				HitDistanceSmallerEqual (buildingSlopeHitDR [0], maxSlopeHigh) &&
				HitDistanceSmallerEqual (buildingSlopeHitM [0], maxSlopeHigh)) {
				return true;
			} else
				return false;
		} else {

			return false;
		}
	}

    private bool ContainsTag(RaycastHit[] hitArr, string tag)
    {
        foreach (RaycastHit h in hitArr)
            if (h.collider.tag == tag)
                return true;

        return false;
    }

    private bool ContainsTag(RaycastHit[] hitArr, string tag, out RaycastHit correctHit)
    {
        foreach (RaycastHit h in hitArr)
            if (h.collider.tag == tag)
            {
                correctHit = h;
                return true;
            }

        correctHit = new RaycastHit();
        return false;
    }

    private bool HitDistanceSmallerEqual(RaycastHit hit, float val)
    {
		if (hit.distance - (ghost.transform.localScale.y) <= val)
            return true;

        return false;
    }

    static void ClearConsole()
    {
        // This simply does "LogEntries.Clear()" the long way:
        var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }
}
	