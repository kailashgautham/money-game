using UnityEngine;
using System.Collections;




public class MoveCamera : MonoBehaviour 
{
	//
	// VARIABLES
	//
	
	public float turnSpeed = 4.0f;		// Speed of camera turning when mouse moves in along an axis
	public float panSpeed = 40.0f;		// Speed of the camera when being panned
	public float zoomSpeed = 800.0f;		// Speed of the camera going back and forth
	
	private Vector3 mouseOrigin;	// Position of cursor when mouse dragging starts
	private bool isPanning;		// Is the camera being panned?
	private bool isRotating;	// Is the camera being rotated?
	private bool isZooming;		// Is the camera zooming?
	
	//
	// UPDATE
	//
	
	void Update () 
	{
		float xInput, yInput;
		if (Application.loadedLevelName.Equals ("pranav_2")) {
			xInput = Input.GetAxis ("Vertical") * -1;
			yInput = Input.GetAxis ("Horizontal");
		} else {
			xInput = Input.GetAxis ("Horizontal");
			yInput = Input.GetAxis ("Vertical");
		}

		float scroll = Input.GetAxis("Mouse ScrollWheel");

		// Get the left mouse button
		if(Input.GetMouseButtonDown(0))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isRotating = true;
		}
		
		// Get the right mouse button
		if(xInput != 0 || yInput != 0 )
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isPanning = true;
		}
		
		// Get the middle mouse button
		if(scroll != 0)
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isZooming = true;
		}
		
		// Disable movements on button release
		if (!Input.GetMouseButton(0)) isRotating=false;
		if (xInput == 0 && yInput == 0) isPanning=false;
		if (scroll == 0) isZooming=false;
		
		// Rotate camera along X and Y axis
		if (isRotating)
		{
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
			
			transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
			transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
		}
		
		// Move the camera on it's XY plane
		if (isPanning)
		{
			//Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
			Vector3 pos = new Vector3(-yInput, 0, xInput);

			Vector3 move = new Vector3(pos.x * panSpeed, pos.y * panSpeed, pos.z * panSpeed);
			transform.Translate(move * Time.deltaTime, Space.World);
		}
		
		// Move the camera linearly along Z axis
		if (isZooming)
		{
			//Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
			
			Vector3 move = scroll * zoomSpeed * transform.forward;//pos.y * zoomSpeed * transform.forward; 
			transform.Translate(move* Time.deltaTime , Space.World);
		}
	}
}