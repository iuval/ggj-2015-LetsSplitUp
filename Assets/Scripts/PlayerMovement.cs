using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float accel = 0;
	public bool touchingRightWall = false;
	public bool touchingLeftWall = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 pos = gameObject.transform.position;
		pos.x += accel - WorldController.floorAccel;
		gameObject.transform.position = pos;
	}
	
	public void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.name == "RightWall") {
			touchingRightWall = true;
		} else if (collision.gameObject.name == "LeftWall") {
			touchingLeftWall = true;
		}
	}
	
	public void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.name == "RightWall") {
			touchingRightWall = false;
		} else if (collision.gameObject.name == "LeftWall") {
			touchingLeftWall = false;
		}
	}
}
