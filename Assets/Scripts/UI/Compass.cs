using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float value = -player.transform.eulerAngles.y;
		transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, value));
	}
}
