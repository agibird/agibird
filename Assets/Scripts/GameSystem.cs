using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour {

	public Transform sphere;

	public GameObject player;

	static private int nSpheres = 16;

	private Transform[] spheres = new Transform[nSpheres];

	// Use this for initialization
	void Start () {
		createSpheres ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
}
