using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;

public class KinectManager : MonoBehaviour {

	private bool printed = false;

	public GameObject BodySourceManager;

	private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
	private BodySourceManager _BodyManager;

	private float angle = 0;

	private float limitAngle = 45.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(printed == false) {
			Debug.Log("The Kinect Controller is used.");
			printed = true;
		}

		if (BodySourceManager == null)
		{
			return;
		}

		_BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
		if (_BodyManager == null)
		{
			return;
		}

		Kinect.Body[] data = _BodyManager.GetData();
		if (data == null)
		{
			return;
		}

		List<ulong> trackedIds = new List<ulong>();
		foreach(var body in data)
		{
			if (body == null)
			{
				continue;
			}

			if(body.IsTracked)
			{
				trackedIds.Add (body.TrackingId);
			}
		}

		List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

		// First delete untracked bodies
		foreach(ulong trackingId in knownIds)
		{
			if(!trackedIds.Contains(trackingId))
			{
				Destroy(_Bodies[trackingId]);
				_Bodies.Remove(trackingId);
			}
		}

		foreach(var body in data)
		{
			if (body == null)
			{
				continue;
			}

			if(body.IsTracked)
			{
				if(!_Bodies.ContainsKey(body.TrackingId))
				{
					_Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
				}

				RefreshBodyObject(body, _Bodies[body.TrackingId]);
			}
		}
	}





	void FixedUpdate() {
		// TODO: Better game controller.
		gameObject.transform.Translate(gameObject.transform.forward * 5.0f);
		gameObject.transform.Rotate (new Vector3 (0, angle * Time.deltaTime, 0));
	}





	private GameObject CreateBodyObject(ulong id)
	{
		GameObject body = new GameObject("Body:" + id);

		Kinect.JointType lht = Kinect.JointType.HandLeft;
		Kinect.JointType rht = Kinect.JointType.HandRight;

		GameObject lhObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject rhObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

		LineRenderer lr = lhObj.AddComponent<LineRenderer>();
		lr.SetVertexCount(2);
		//lr.material = BoneMaterial;
		lr.SetWidth(0.05f, 0.05f);

		lhObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
		lhObj.name = lht.ToString();
		lhObj.transform.parent = body.transform;

		rhObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
		rhObj.name = rht.ToString();
		rhObj.transform.parent = body.transform;

		return body;
	}





	private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
	{

		Kinect.JointType lht = Kinect.JointType.HandLeft;
		Kinect.JointType rht = Kinect.JointType.HandRight;

		Kinect.Joint lh = body.Joints [Kinect.JointType.HandLeft];
		Kinect.Joint rh = body.Joints [Kinect.JointType.HandRight];
		//Debug.Log (lh);
		//Debug.Log (Mathf.Round(lh.Position.Y * 10) + "     " + Mathf.Round( rh.Position.Y * 10));

		// Kinect coordinates scale.
		float scale = 100.0f;

		// Get the x and y positions of the left and right hand.
		float lhy = lh.Position.Y * scale;
		float rhy = rh.Position.Y * scale;
		float lhx = lh.Position.X * scale;
		float rhx = rh.Position.X * scale;

		// Get the amount of tilt between the left and right hand.
		Vector3 lhVector = new Vector3 (lhx, 0, lhy);
		Vector3 rhVector = new Vector3 (rhx, 0, rhy);
		Vector3 tilt = rhVector - lhVector;
		Vector3 horizontal = new Vector3 (1, 0, 0);
		angle = Vector3.Angle (horizontal, tilt);

		// Limit the angle.
		angle = Mathf.Min (angle, limitAngle);

		if(angle < 5.0f) {
			angle = 0.0f;
		}

		//TODO: Use physics.

		// Tilt the player object.
		if(lhy < rhy) {
			angle = -angle;
			//gameObject.transform.eulerAngles = new Vector3 (0, 0, -angle);
			//gameObject.transform.Rotate (new Vector3 (0, angle * Time.deltaTime, 0));
		} else {
			//gameObject.transform.eulerAngles = new Vector3 (0, 0, angle);
			//gameObject.transform.Rotate (new Vector3 (0, -angle * Time.deltaTime, 0));
		}
		Debug.Log (angle);

	}





	private static Color GetColorForState(Kinect.TrackingState state)
	{
		switch (state)
		{
		case Kinect.TrackingState.Tracked:
			return Color.green;

		case Kinect.TrackingState.Inferred:
			return Color.red;

		default:
			return Color.black;
		}
	}





	private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
	{
		return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
	}





}
