using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public bool canHitHard = false;
	public bool canJumpHigh = false;

	public float accel = 0;
	public bool touchingRightWall = false;
	public bool touchingLeftWall = false;
	public bool touchingFloor = false;

	public bool wantsToChange = false;

	private GameObject obstacle;

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
		} else if (collision.gameObject.tag == "Floor") {
			touchingFloor = true;
		} else if (collision.gameObject.tag == "Obstacle") {
			obstacle = collision.gameObject;
		}
	}
	
	public void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.name == "RightWall") {
			touchingRightWall = false;
		} else if (collision.gameObject.name == "LeftWall") {
			touchingLeftWall = false;
		} else if (collision.gameObject.tag == "Floor") {
			touchingFloor = false;
		} else if (collision.gameObject.tag == "Obstacle") {
			obstacle = null;
		}
	}

	public void Jump() {
		if (touchingFloor) {
			Vector2 vel = rigidbody2D.velocity;
			if (canJumpHigh) {
				vel.y += WorldController.playerJumpSpeed;
			} else {
				vel.y += WorldController.playerJumpSpeed * 1.5f;
			}
			rigidbody2D.velocity = vel;
		}
	}

	public void Hit() {
		if (canHitHard && obstacle) {
			BoxCollider2D box = (BoxCollider2D)obstacle.collider2D;
			Vector3 center = box.center;
			center.y -= box.size.y / 2;
			box.center = center;
			box.size = new Vector3(2f, 0.2f, 0);
		}
	}
}
