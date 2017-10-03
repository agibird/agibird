using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour {

	public KinectManager kinectController;

	public KeyboardManager keyboardController;

	private Rigidbody rigidBody;

	private float pitch, yaw;

	public enum GameController {
		KinectController,
		KeyboardController
	};

	public GameController gameController;

	// Use this for initialization
	void Start () {
		if(gameController == GameController.KinectController) {
			keyboardController.enabled = false;
			kinectController.enabled = true;
		} else if(gameController == GameController.KeyboardController) {
			kinectController.enabled = false;
			keyboardController.enabled = true;
		}

		rigidBody = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate () {
		Vector3 forward = transform.forward.normalized;
		Vector3 up = transform.up.normalized;

		float velocityMagnitude = rigidBody.velocity.magnitude;

		AlignTowardsVelocity ();


		//roll
		float roll = 0f;
		if(Input.GetKey("left")){
			roll = +1.5f;
		}
		if(Input.GetKey("right")){
			roll = -0.75f;
		}

		roll *= 100f * velocityMagnitude * Time.fixedDeltaTime;

		rigidBody.AddRelativeTorque (0, 0, roll);



		float liftMod = 1f;
		if(Input.GetKey("down")){
			liftMod = +2;
		}
		if(Input.GetKey("up")){
			liftMod = -1.25f;
		}
		
		
		
		//gravity
		rigidBody.velocity += 10 * new Vector3(0, -1, 0) * Time.fixedDeltaTime;

		//Poop engine
		rigidBody.velocity += 30 * forward * Time.fixedDeltaTime;

		//lift
		float lift = 0.15f * liftMod * rigidBody.velocity.magnitude * Time.fixedDeltaTime;
		rigidBody.velocity += up*lift - forward*Mathf.Max(0f, lift); //allow negative lift, but not negative drag.
	}

	void AlignTowardsVelocity () {
		
		var targetDir = rigidBody.velocity;
		var forward = rigidBody.transform.forward;
		var localTarget = rigidBody.transform.InverseTransformDirection(targetDir);

		float xz = Mathf.Sqrt(localTarget.x * localTarget.x + localTarget.z * localTarget.z);

		float yaw = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
		float pitch = Mathf.Atan2(localTarget.y, xz) * Mathf.Rad2Deg;

		Vector3 eulerAngleVelocity = 400f * new Vector3 (-pitch, yaw, 0) * Time.fixedDeltaTime;

		rigidBody.AddRelativeTorque (eulerAngleVelocity);

	}
}
