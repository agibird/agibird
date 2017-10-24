using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speed : MonoBehaviour {

	// The player.
	public GameObject player;

	// The UI text which displays the speed.
	private Text text;

	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "Speed: " + (player.GetComponent<Rigidbody> ().velocity.magnitude * 3.6f).ToString("00");
	}
}
