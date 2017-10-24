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

	private float foldDistance = 0.16f;

	private float stretchDistance = 0.39f;

	private KinectManager kinectManager;

	private bool stretched = true;





	// Use this for initialization
	void Start () {
		kinectManager = player.GetComponent<KinectManager> ();
		animator = gameObject.GetComponent<Animator> ();
		armLength = 0.5f;
		animator.speed = 0f;
	}





	// Update is called once per frame
	void FixedUpdate () {

		// Use keyboard input.
		if(Input.GetKey("q") && stretched == true) {
			PlayFold ();
			stretched = false;
		}else if(Input.GetKey("e") && stretched == false) {
			PlayStretch ();
			stretched = true;
		}

		// Use Kinect input.
		if(kinectManager.tracking() == true) {
			float average = (player.GetComponent<KinectManager> ().getDistanceLeftHandToHip() + player.GetComponent<KinectManager> ().getDistanceRightHandToHip()) / 2.0f;

			// If hands close to hips, play folding animation. If hands not close to hips, play stretching animation.
			if(average < foldDistance && stretched == true) {
				PlayFold ();
				stretched = false;
			} else if(average > stretchDistance && stretched == false) {
				PlayStretch ();
				stretched = true;
			}
		}
	}





	/// <summary>
	/// Sets the animation frame. Use this to change which part of the bird animation that is displayed.
	/// Frame should be between 0 and 1.
	/// </summary>
	public void SetAnimation (float frame) {
		//animationFrame = frame;
		animator.speed = 0f;
		animator.Play ("Fold", 0, frame);
	}





	/// <summary>
	/// Playes the folding animation.
	/// </summary>
	public void PlayFold() {
		animator.speed = 5.0f;
		animator.Play ("Fold");
	}





	/// <summary>
	/// Playes the stretching animation.
	/// </summary>
	public void PlayStretch() {
		animator.Play ("Stretch");
	}





}
