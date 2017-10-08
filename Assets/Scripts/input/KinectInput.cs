using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinectInput : MonoBehaviour, InputHandler {

	public KinectManager kinectManager;

	public void update(){
		
	}

	public float getYaw() {

		// The vector between the hands.
		Vector3 vectorBetweenHands = kinectManager.getVectorBetweenHands ();

		// A horizontal vector.
		Vector3 horizontal = new Vector3 (vectorBetweenHands.x, 0, vectorBetweenHands.z);

		// The angle of the vector between the hands.
		float angle = Vector3.Angle (horizontal, vectorBetweenHands);

		// The angle limit on how much you can tilt your arms.
		float limit = 45.0f;

		float value = angle / limit;

		if(vectorBetweenHands.y > 0) {
			value *= -1;
		}

		return value;
	}

	public float getPitch() {

		// The vector indicating how much you lean.
		Vector3 leaningVector = kinectManager.getLeaningVector ();

		// A vertical vector.
		Vector3 vertical = Vector3.up;

		// How much you lean.
		float angle = Vector3.Angle (vertical, leaningVector);

		// The angle limit on how much you can lean.
		float limit = 25.0f;

		float value = angle / limit;

		if(leaningVector.z > 0) {
			value *= -1;
		}

		return value;
	}

	public float getRoll() {
		
		// The vector between the hands.
		Vector3 vectorBetweenHands = kinectManager.getVectorBetweenHands ();

		// A horizontal vector.
		Vector3 horizontal = new Vector3 (vectorBetweenHands.x, 0, vectorBetweenHands.z);

		// The angle of the vector between the hands.
		float angle = Vector3.Angle (horizontal, vectorBetweenHands);

		// The angle limit on how much you can tilt your arms.
		float limit = 45.0f;

		float value = angle / limit;

		if(vectorBetweenHands.y > 0) {
			value *= -1;
		}

		return value;
	}
}
