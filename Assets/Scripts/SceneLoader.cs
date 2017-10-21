using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

	// The game object which has the loader graphics.
	public GameObject loader;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Load the main scene.
	public void startGame() {
		StartCoroutine (loadScene ("Main"));
	}


	// Load the main menu.
	public void loadMainMenu() {
		StartCoroutine(loadScene("MainMenu"));
	}

	IEnumerator loadScene(string scene) {

		AsyncOperation ao = SceneManager.LoadSceneAsync (scene);

		while(ao.isDone == false) {
			float prog = ao.progress;
			//slider.value = prog;
			if (loader != null) {
				loader.GetComponent<Image> ().fillAmount = prog;
			}
			yield return null;
		}

		yield return null;
	}

	// Quit the game.
	public void quitGame() {

		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
