using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnimation : MonoBehaviour {

	// The raven animator.
	private Animator animator;

	// The frame in the animation to display. The animation time scale is 0 to 1.
	private float animationFrame = 0;





	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator> ();
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
		animationFrame += Input.GetAxis ("Horizontal") / 100.0f;
	}





	/// <summary>
	/// Sets the animation frame. Use this to change which part of the bird animation that is displayed.
	/// Frame should be between 0 and 1.
	/// </summary>
	public void setAnimation (float frame) {
		animationFrame = frame;
	}





}
