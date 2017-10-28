/// <summary>
/// Scene loader. Handles scene loading and application quitting.
/// Author: Jonatan Cöster
/// Created: 2017-10-08
/// Version: 1.0
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

	// The game object which has the loader graphics.
	public GameObject loader;





	/// <summary>
	/// Starts the game by loading the main scene.
	/// </summary>
	public void startGame() {
		StartCoroutine (loadScene ("Main"));
	}





	/// <summary>
	/// Loads the main menu.
	/// </summary>
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





	/// <summary>
	/// Quits the game.
	/// </summary>
	public void quitGame() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
