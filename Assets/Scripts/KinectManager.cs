using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;

public class KinectManager : MonoBehaviour {

	public GameObject BodySourceManager;

	private BodySourceManager _BodyManager;

	// Kinect coordinate system scale.
	private float scale = 100.0f;





	// Use this for initialization
	void Start () {
		if (BodySourceManager == null) {
			return;
		}

		_BodyManager = BodySourceManager.GetComponent<BodySourceManager> ();
		if (_BodyManager == null) {
			return;
		}
	}





	/// <summary>
	/// Returns the vector from the left hand to the right hand. This vector is always returned from the first of the tracked objects, 
	/// regardless of how many objects are tracked. If no object is tracked, this will return the zero vector.
	/// </summary>
	/// <returns>The vector from the left hand to the right hand.</returns>
	public Vector3 getVectorBetweenHands() {
		
		Kinect.Body[] data = _BodyManager.GetData();
		if (data == null) {
			Debug.Log("Not tracking");
			return Vector3.zero;
		}

		foreach (var body in data) {
			if (body == null) {
				continue;
			}

			if (body.IsTracked) {

				Kinect.JointType lht = Kinect.JointType.HandLeft;
				Kinect.JointType rht = Kinect.JointType.HandRight;

				Kinect.Joint lh = body.Joints [Kinect.JointType.HandLeft];
				Kinect.Joint rh = body.Joints [Kinect.JointType.HandRight];

				// Get the x and y positions of the left and right hand.
				float lhy = lh.Position.Y * scale;
				float rhy = rh.Position.Y * scale;
				float lhx = lh.Position.X * scale;
				float rhx = rh.Position.X * scale;
				float lhz = lh.Position.Z * scale;
				float rhz = rh.Position.Z * scale;

				// Get the vector between the left and right hand.
				Vector3 lhVector = new Vector3 (lhx, lhy, -lhz);
				Vector3 rhVector = new Vector3 (rhx, rhy, -rhz);
				Vector3 handVector = rhVector - lhVector;

				return handVector;
			}
		}

		Debug.Log("Not tracking");
		return Vector3.zero;

	}





	/// <summary>
	/// Returns a vector representing how the person is leaning. This vector is always returned from the first of the tracked objects, 
	/// regardless of how many objects are tracked. If no object is tracked, this will return the zero vector.
	/// </summary>
	/// <returns>The vector representing how the person is leaning.</returns>
	public Vector3 getLeaningVector() {
		Kinect.Body[] data = _BodyManager.GetData();
		if (data == null) {
			Debug.Log("Not tracking");
			return Vector3.zero;
		}

		foreach (var body in data) {
			if (body == null) {
				continue;
			}

			if (body.IsTracked) {

				Kinect.JointType lht = Kinect.JointType.HipLeft;
				Kinect.JointType ht = Kinect.JointType.Head;

				Kinect.Joint lh = body.Joints [Kinect.JointType.HipLeft];
				Kinect.Joint h = body.Joints [Kinect.JointType.Head];

				// Get the x and y positions of the left and right hand.
				float lhy = lh.Position.Y * scale;
				float hy = h.Position.Y * scale;
				float lhx = lh.Position.X * scale;
				float hx = h.Position.X * scale;
				float lhz = lh.Position.Z * scale;
				float hz = h.Position.Z * scale;

				// Get the vector between the left and right hand.
				Vector3 lhVector = new Vector3 (lhx, lhy, lhz);
				Vector3 hVector = new Vector3 (hx, hy, hz);
				Vector3 leaningVector = hVector - lhVector;

				return leaningVector;
			}
		}

		Debug.Log("Not tracking");
		return Vector3.zero;
	}





}
