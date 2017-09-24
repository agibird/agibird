using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour {

	public KinectManager kinectController;

	public KeyboardManager keyboardController;

	public enum GameController {
		KinectController,
		KeyboardController
	};

	public GameController gameController;

	// Use this for initialization
	void Start () {
		if(gameController == GameController.KinectController) {
			keyboardController.enabled = false;
			kinectController.enabled = true;
		} else if(gameController == GameController.KeyboardController) {
			kinectController.enabled = false;
			keyboardController.enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
