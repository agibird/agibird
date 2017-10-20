using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

	public Slider slider;

	public GameObject loader;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startGame() {
		StartCoroutine (loadScene ());
	}

	IEnumerator loadScene() {

		AsyncOperation ao = SceneManager.LoadSceneAsync ("Main");

		while(ao.isDone == false) {
			float prog = ao.progress;
			//slider.value = prog;
			loader.GetComponent<Image> ().fillAmount = prog;
			Debug.Log (prog);
			yield return null;
		}

		yield return null;
	}

	public void quitGame() {

		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
