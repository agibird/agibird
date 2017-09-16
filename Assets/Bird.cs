using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

	//private Camera camera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate(){
		transform.RotateAround (new Vector3 (500, 0, 500), new Vector3 (0, 1, 0), -0.1f);
		if (Input.GetButtonDown("Fire1"))
		{
			Debug.Log(Input.mousePosition);
		}

		if(Input.GetKeyDown("a"))
		{
			Debug.Log ("A pressed");
			//float translation = Time.deltaTime * 100;
			//transform.Rotate(Vector3.right, translation, Space.World);

		}
		if(Input.GetKeyDown("d"))
		{
			Debug.Log ("D");	
		}
	}
}
