using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSliderValue : MonoBehaviour {

	public GameObject slider;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float playTime = slider.GetComponent<Slider> ().value;
		string units = " Minutes";
		if(playTime == 1) {
			units = " Minute";
		}
		gameObject.GetComponent<Text> ().text = "Play time: " + playTime.ToString() + units;
	}
}
