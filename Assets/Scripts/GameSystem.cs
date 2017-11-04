/// <summary>
/// Handles all aspects of the gameplay such as keeping track of time and points. Also updates relevant parts of the UI.
/// Author: Jonatan Cöster
/// Created: 2017-10-14
/// Version 1.0
/// Additions: Sound, added by Yinan Xu, 2017-10-25
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour {

	public Transform sphere;

	// The player.
	public GameObject player;

	// The game object which has the UI timer.
	public GameObject uiTimer;

	// The UI panel which is displayed at the end of a round.
	public GameObject endRoundPanel;

	public GameObject gameOverPanel;

	public GameObject pausePanel;

	static private int nSpheres = 16;

	private Transform[] spheres = new Transform[nSpheres];

	// The starting time of the current round.
	private float startTime;

	// The available play time.
	private float playTime;





	// Use this for initialization
	void Start () {
		GameSettings currentSettings =
			GameSettingsToDiskInterface.LoadOrDefault();

		playTime = currentSettings.PlayTime * 60.0f;
		Time.timeScale = 1.0f;
		startTime = Time.time;
		FindObjectOfType<AudioManager>().Play("Wind");
	}





	// Update is called once per frame
	void Update () {
		checkTime ();
		checkPauseButton ();
	}





	/// <summary>
	/// Creates hovering spheres in the scene.
	/// </summary>
	private void createSpheres() {
		for (int i = 0; i < nSpheres; i++) {
			Vector3 playerPosition = player.transform.position;

			int range = 150;

			int x = Random.Range (-range, range);
			int z = Random.Range (-range, range);

			Vector3 spherePosition = new Vector3 (playerPosition.x + x, playerPosition.y, playerPosition.z + z);

			spheres[i] = Instantiate (sphere, spherePosition, Quaternion.identity);
		}
	}





	/// <summary>
	/// Checks if there is any playtime remaining. If no playtime remains this pauses the game
	/// and displays a "game over" panel.
	/// </summary>
	private void checkTime() {
		float usedTime = Time.time - startTime;
		float remainingTime = playTime - usedTime;

		if(remainingTime <= 0f) {
			Time.timeScale = 0f;
			endRoundPanel.SetActive (true);
		}

		updateUITimer (remainingTime);
	}





	public void displayGameOver() {
		Time.timeScale = 0f;
		gameOverPanel.SetActive(true);
		FindObjectOfType<AudioManager>().Play("BirdDeath");
		FindObjectOfType<AudioManager>().Stop("Wind");
	}





	/// <summary>
	/// Updates the user interface timer.
	/// </summary>
	/// <param name="time">Time.</param>
	private void updateUITimer(float time) {
		int min = (int)time / 60;
		int sec = (int)time % 60;
		uiTimer.GetComponent<Text>().text = "Time: " + min.ToString() + ":" + sec.ToString("00");
	}





	/// <summary>
	/// Resets the player's points.
	/// </summary>
	private void resetPoints() {
		PlayerPrefs.SetInt ("Player Score", 0);
	}





	/// <summary>
	/// Adds "number" player points.
	/// </summary>
	public void addPoints(int number) {
		int currentPoints = PlayerPrefs.GetInt ("Player Score");
		PlayerPrefs.SetInt ("Player Score", currentPoints + number);
	}





	private void checkPauseButton() {
		if(Input.GetKeyDown("escape")) {
			Time.timeScale = 0f;
			pausePanel.SetActive (true);
			FindObjectOfType<AudioManager>().Pause("Wind");
		}
	}





	public void resumeGame() {
		Time.timeScale = 1.0f;
		FindObjectOfType<AudioManager>().Play("Wind");
	}

}
