using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool IsOutside() {
		return gameObject.transform.position.x < -WorldController.distanceToDestroyObstacle;
	}
}
