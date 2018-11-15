using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	[SerializeField]
	private float cameraSpeed = 0.0f;

	private float xMax;
	private float yMin;

	// Update is called once per frame
	private void Update () {
		getInput ();
	}

	private void getInput(){
		//up
		if(Input.GetKey(KeyCode.W)){
			transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
		}
		//left
		if(Input.GetKey(KeyCode.A)){
			transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
		}
		//down
		if(Input.GetKey(KeyCode.S)){
			transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);
		}
		//right
		if(Input.GetKey(KeyCode.D)){
			transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
		}

		transform.position = new Vector3 (Mathf.Clamp (transform.position.x, 0, xMax),
			Mathf.Clamp (transform.position.y, yMin, 0), -10);
	}

	public void setLimits(Vector3 maxTile){
		Vector3 bottomRight = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0));

		xMax = maxTile.x - bottomRight.x;
		yMin = maxTile.y - bottomRight.y;
	}
}
