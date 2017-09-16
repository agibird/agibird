using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	public Camera camera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float translation = Time.deltaTime * 40;

		if(Input.GetKey("a"))
		{
			//transform.Rotate(Vector3.forward, translation, Space.World);
			transform.Rotate(Vector3.down, translation, Space.World);


		}
		if(Input.GetKey("d"))
		{
			//transform.Rotate(Vector3.back, translation, Space.World);
			transform.Rotate(Vector3.up, translation, Space.World);

		}
	}
}
