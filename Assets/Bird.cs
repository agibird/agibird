using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		transform.RotateAround (new Vector3 (500, 0, 500), new Vector3 (0, 1, 0), -0.1f);
	}
}
