using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

	public GameObject playTime;

	public void SaveSettings() {
		Slider slider = playTime.GetComponent<Slider> ();
		float value = slider.value * 60.0f;
		PlayerPrefs.SetFloat ("PlayTime", value);
	}
}
