using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour {

	private bool printed = false;

	private float tilt = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(printed == false) {
			Debug.Log("The Keyboard Controller is used.");
			printed = true;
		}
	}

	void FixedUpdate() {
		gameObject.GetComponent<Rigidbody> ().MovePosition (transform.position + transform.forward * Time.deltaTime * 50.0f);

		float rotation = Input.GetAxis ("Horizontal") * 10.0f;
		rotation *= Time.deltaTime;

		tilt = Mathf.Min (rotation, 45.0f);

		gameObject.transform.Rotate (0, rotation, 0);
		transform.Find("bird").localEulerAngles = new Vector3(0, 0, -tilt * 100.0f);
	}
}
