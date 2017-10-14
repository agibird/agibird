using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour {

	private float position;

	private float add;

	// Use this for initialization
	void Start () {
		position = transform.position.y;
		add = Random.Range (0f, 5.0f);
		StartCoroutine (hover ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {

	}

	IEnumerator hover() {
		while(true) {
			float value = position + 5.0f *Mathf.Sin (add + 0.5f * Time.time);
			transform.position = new Vector3 (transform.position.x, value, transform.position.z);
			yield return null;
		}
	}
}
