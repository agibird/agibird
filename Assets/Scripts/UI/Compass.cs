using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to display the player orientation on the compass.
/// </summary>

public class Compass : MonoBehaviour {

	// The player.
	public GameObject player;

	// Update is called once per frame
	void Update () {
		float value = -player.transform.eulerAngles.y;
		transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, value));
	}
}
