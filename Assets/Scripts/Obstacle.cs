using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	GameObject spikes;
	GameObject wall;

	// Use this for initialization
	void Start () {
		foreach (Transform t in transform) {
			if (t.name == "Spikes") spikes = t.gameObject;
			if (t.name == "Wall") wall = t.gameObject;
		}
		spikes.SetActive (false);
		wall.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool IsOutside() {
		return gameObject.transform.position.x < -WorldController.distanceToDestroyObstacle;
	}
}
