using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinectManager : MonoBehaviour {

	private bool printed = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(printed == false) {
			Debug.Log("The Kinect Controller is used.");
			printed = true;
		}
	}
}
