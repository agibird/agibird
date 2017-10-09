using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour {

	public KinectManager kinectManager;

	public int sensitivity = 100;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		Debug.Log (kinectManager.tracking ());

		Vector3 vectorBetweenHands = kinectManager.getVectorBetweenHands ();
		Vector3 horizontal = new Vector3 (vectorBetweenHands.x, 0, vectorBetweenHands.z);

		float angle = Vector3.Angle (horizontal, vectorBetweenHands);
		if(vectorBetweenHands.y < 0) {
			angle *= -1;
		}

		Quaternion direction = Quaternion.AngleAxis (angle/(float)sensitivity, -Vector3.up);

		Vector3 leaningVector = kinectManager.getLeaningVector ();
		float leaning = Vector3.Angle (Vector3.up, leaningVector);
		if(leaningVector.z < 0) {
			leaning *= -1;
		}

		if(kinectManager.tracking()) {
			gameObject.GetComponent<Rigidbody> ().MoveRotation (direction * transform.rotation);
			transform.Find ("bird").localEulerAngles = new Vector3 (0f, 0f, angle);
			gameObject.GetComponent<Rigidbody> ().MovePosition (transform.position + transform.forward * Time.fixedDeltaTime * 12.0f);
		}
	}
}
