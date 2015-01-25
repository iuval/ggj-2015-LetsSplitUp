using UnityEngine;
using System.Collections;

public class DarknessController : MonoBehaviour {

	public bool isVisible = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (WorldController.playing) {
			Vector3 pos = gameObject.transform.position;
			pos.x += WorldController.darknessSpeed - WorldController.floorAccel;
			gameObject.transform.position = pos;
		}
	}
	
	public void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.name == "LefttWall") {
			isVisible = true;
		}
	}
	
	public void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.name == "LefttWall") {
			isVisible = false;
		}
	}
}
