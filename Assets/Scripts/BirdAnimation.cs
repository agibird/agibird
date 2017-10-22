using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnimation : MonoBehaviour {

	public GameObject player;

	// The raven animator.
	private Animator animator;

	// The frame in the animation to display. The animation time scale is 0 to 1.
	private float animationFrame = 0;

	private float armLength;





	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator> ();
		armLength = 0.5f;
	}





	// Update is called once per frame
	void Update () {
		animator.speed = 0f;
		animator.Play ("New State", 0, animationFrame);
	}





	void FixedUpdate() {
		// Control the animation with the keyboard.
		if(Input.GetKey("q")) {
			animationFrame -= 0.1f;
		} else if(Input.GetKey("e")) {
			animationFrame += 0.1f;
		}

		// Use the kinect to control the animation.
		if(player.GetComponent<KinectManager>().tracking() == true) {
			// Get the average arm length.
			float average = (player.GetComponent<KinectManager> ().getDistanceLeftHandToShoulder() + player.GetComponent<KinectManager> ().getDistanceRightHandToShoulder()) / 2.0f;

			float value = 1.0f - (average / armLength);
			setAnimation (value);
		}
	}





	/// <summary>
	/// Sets the animation frame. Use this to change which part of the bird animation that is displayed.
	/// Frame should be between 0 and 1.
	/// </summary>
	public void setAnimation (float frame) {
		animationFrame = frame;
	}





}
