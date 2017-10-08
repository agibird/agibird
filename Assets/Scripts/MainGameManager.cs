using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour {

	public bool keyboardInput;
	public bool useKinectInput;
	public bool controllerInput;

	public KinectInput kinectInput;

	private List<InputHandler> inputHandlers;

	private Rigidbody rigidBody;

	private float pitch, yaw;

	// Use this for initialization
	void Start () {

		inputHandlers = new List<InputHandler>();
		if (keyboardInput) {
			inputHandlers.Add (new KeyboardInput ());
		}
		if (useKinectInput) {
			inputHandlers.Add (kinectInput);
		}
		if (controllerInput) {
			inputHandlers.Add (new ControllerInput ());
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


		float yaw = 0f;
		float pitch = 0f;
		float roll = 0f;
		for (int i = 0; i < inputHandlers.Count; i++) {
			InputHandler ih = inputHandlers[i];

			if (ih == null) {
				continue;
			}

			ih.update ();

			yaw += ih.getYaw ();
			pitch += ih.getPitch ();
			roll += ih.getRoll ();
		}

		yaw = Mathf.Clamp (yaw, -1, +1);
		pitch = Mathf.Clamp (pitch, -1, +1);
		roll = Mathf.Clamp (roll, -1, +1);



		float rollTorque = roll * 300f * velocityMagnitude * Time.fixedDeltaTime;
		rigidBody.AddRelativeTorque (0, 0, rollTorque);




		float gravityStrength = 30;
		
		//gravity
		rigidBody.velocity += gravityStrength * new Vector3(0, -1, 0) * Time.fixedDeltaTime;

		//Poop engine
		rigidBody.velocity += 20 * forward * Time.fixedDeltaTime;

		//rigidBody.velocity += up*lift - forward*Mathf.Max(lift, 0.0f); //Allow negative lift, but not negative drag


		//lift+pitch

		float lift = Mathf.Min(gravityStrength, 0.5f * rigidBody.velocity.magnitude) * Time.fixedDeltaTime;
		float pitchForce = 0.5f * -pitch * rigidBody.velocity.magnitude * Time.fixedDeltaTime;

		float speed = rigidBody.velocity.magnitude;
		rigidBody.velocity = (rigidBody.velocity + up * (lift + pitchForce)).normalized * speed;

	}

	void AlignTowardsVelocity () {
		
		var targetDir = rigidBody.velocity;
		var forward = rigidBody.transform.forward;
		var localTarget = rigidBody.transform.InverseTransformDirection(targetDir);

		float xz = Mathf.Sqrt(localTarget.x * localTarget.x + localTarget.z * localTarget.z);

		float yaw = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
		float pitch = Mathf.Atan2(localTarget.y, xz) * Mathf.Rad2Deg;

		Vector3 eulerAngleVelocity = 1000f * new Vector3 (-pitch, yaw, 0) * Time.fixedDeltaTime;

		rigidBody.AddRelativeTorque (eulerAngleVelocity);

	}
}
