using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;

public class KinectManager : MonoBehaviour {

	public GameObject BodySourceManager;

	private BodySourceManager _BodyManager;

	// Whether the Kinect is tracking an object.
	private bool isTracking;

	// Kinect coordinate system scale.
	private float scale = 100.0f;





	// Use this for initialization
	void Start () {
		this.isTracking = false;
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
			return Vector3.zero;
		}

		foreach (var body in data) {
			if (body == null) {
				continue;
			}

			if (body.IsTracked) {

				this.isTracking = true;

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
			return Vector3.zero;
		}

		foreach (var body in data) {
			if (body == null) {
				continue;
			}

			if (body.IsTracked) {

				this.isTracking = true;

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

		return Vector3.zero;
	}





	/// <summary>
	/// Gets the distance from the left hand to the left shoulder.
	/// </summary>
	/// <returns>The distance from the left hand to the left shoulder.</returns>
	public float getDistanceLeftHandToShoulder() {
		float distance = getDistanceBetweenJoints (Kinect.JointType.ShoulderLeft, Kinect.JointType.HandLeft);
		return distance;
	}





	/// <summary>
	/// Gets the distance from the right hand to the right shoulder.
	/// </summary>
	/// <returns>The distance from the right hand to the right shoulder.</returns>
	public float getDistanceRightHandToShoulder() {
		float distance = getDistanceBetweenJoints (Kinect.JointType.ShoulderRight, Kinect.JointType.HandRight);
		return distance;
	}





	/// <summary>
	/// Gets the distance between two Kinect joints.
	/// </summary>
	/// <returns>The distance between two Kinect joints.</returns>
	public float getDistanceBetweenJoints(Kinect.JointType from, Kinect.JointType to) {
		
		Kinect.Body[] data = _BodyManager.GetData();
		if (data == null) {
			return 0f;
		}

		foreach (var body in data) {
			if (body == null) {
				continue;
			}

			if (body.IsTracked) {

				this.isTracking = true;

				Kinect.Joint fromJoint = body.Joints [from];
				Kinect.Joint toJoint = body.Joints [to];

				// Get the x and y positions of the left and right hand.
				float fromY = fromJoint.Position.Y;
				float toY = toJoint.Position.Y;
				float fromX = fromJoint.Position.X;
				float toX = toJoint.Position.X;
				float fromZ = fromJoint.Position.Z;
				float toZ = toJoint.Position.Z;

				// Get the vector between the left and right hand.
				Vector3 fromVector = new Vector3 (fromX, fromY, fromZ);
				Vector3 toVector = new Vector3 (toX, toY, toZ);
				Vector3 lengthVector = toVector - fromVector;

				return lengthVector.magnitude;
			}
		}

		return 0f;
	}





	/// <summary>
	/// Gets the distance from the left hand to the left hip.
	/// </summary>
	/// <returns>The distance from the left hand to the left hip.</returns>
	public float getDistanceLeftHandToHip() {
		float distance = getDistanceBetweenJoints (Kinect.JointType.HandLeft, Kinect.JointType.HipLeft);
		return distance;
	}





	/// <summary>
	/// Gets the distance from the right hand to the right hip.
	/// </summary>
	/// <returns>The distance from the right hand to the right hip.</returns>
	public float getDistanceRightHandToHip() {
		float distance = getDistanceBetweenJoints (Kinect.JointType.HandRight, Kinect.JointType.HipRight);
		return distance;
	}





	/// <summary>
	/// Returns true if the Kinect is tracking an object. Returns false otherwise.
	/// </summary>
	public bool tracking() {
		return this.isTracking;
	}





}
