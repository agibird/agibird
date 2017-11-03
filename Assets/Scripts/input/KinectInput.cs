using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinectInput : MonoBehaviour, InputHandler {

	public KinectManager kinectManager;

	public float pitchAngle;

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
		Vector3 l = kinectManager.getLeaningVector ();

		// The lean limit on how much you can lean.
		float limit = 0.7f;

		Vector4 l2 = new Vector4 ();
		l2.Set (l.x, l.y, l.z, 1);

		Matrix4x4 m = Matrix4x4.Rotate (Quaternion.Euler (new Vector3(+pitchAngle, 0, 0)));

		Vector4 v = m * l2;
		//print (m + " * " + l2 + " ---> " + v);

		float f = -v.z / Mathf.Sqrt (v.x * v.x + v.y * v.y + v.z * v.z);


		float value = f / limit;

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
		float limit = 70.0f;

		float value = -angle / limit;

		if(vectorBetweenHands.y > 0) {
			value *= -1;
		}

		return value;
	}
}
