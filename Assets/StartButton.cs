using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour {

	public WorldController controller;

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			controller.StartGame();
		}
	}
}
