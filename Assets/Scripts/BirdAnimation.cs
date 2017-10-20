using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnimation : MonoBehaviour {

	// The raven animator.
	private Animator animator;

	// The frame in the animation to display. The animation time scale is 0 to 1.
	float animationFrame = 0;

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
		if(Input.GetKey("q")) {
			animationFrame -= 0.1f;
		} else if(Input.GetKey("e")) {
			animationFrame += 0.1f;
		}
		animationFrame += Input.GetAxis ("Horizontal") / 100.0f;
	}
}
