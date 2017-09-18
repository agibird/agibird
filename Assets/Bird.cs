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
		float speed = 10.0f;
		transform.RotateAround (new Vector3 (500, 0, 500), new Vector3 (0, 1, 0), -0.1f);
		//transform.Translate(Vector3.forward * Time.deltaTime * speed);

		if(Input.GetKey("a"))
		{
			transform.Rotate (Vector3.down, 0.5f, Space.World);	
		}
		if(Input.GetKey("d"))
		{
			transform.Rotate (Vector3.up, 0.5f, Space.World);	
		}

		if (Input.GetKey ("s")) {
			if (speed >= 5.0f) {
				speed -= 0.5f;
			}
		}
		if (Input.GetKey ("w")) {
			if (speed <= 15.0f) {
				speed += 0.5f;
			}

		}
	}
}
