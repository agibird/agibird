/// <summary>
/// Sphere. Makes a game object hover.
/// Author: Jonatan Cöster
/// Created: 2017-10-14
/// Version: 1.0
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour {

	// The starting position of the sphere.
	private float position;

	// Position offset.
	private float add;

	// Use this for initialization
	void Start () {
		position = transform.position.y;
		add = Random.Range (0f, 5.0f);
		StartCoroutine (hover ());
	}

	IEnumerator hover() {
		while(true) {
			float value = position + 5.0f *Mathf.Sin (add + 0.5f * Time.time);
			transform.position = new Vector3 (transform.position.x, value, transform.position.z);
			yield return null;
		}
	}
}
