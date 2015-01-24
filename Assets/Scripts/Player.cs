using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Animator animator;

	public bool canHitHard = false;
	public bool canJumpHigh = false;

	public float accel = 0;
	public float push = 0;
	public bool touchingRightWall = false;
	public bool touchingLeftWall = false;
	public bool touchingFloor = false;

	public bool wantsToChange = false;

	private GameObject obstacle;

	public CameraControl camera;

	// Use this for initialization
	void Start () {
		animator = GetComponent <Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 pos = gameObject.transform.position;
		pos.x += accel - WorldController.floorAccel + push;
		gameObject.transform.position = pos;
		push = 0;
	}

	public void RunLeft() {
		animator.SetBool ("Run", true);
		transform.localScale = new Vector2 (-1, 1);
	}
	
	public void RunRight() {
		animator.SetBool ("Run", true);
		transform.localScale = new Vector2 (1, 1);
	}
	
	public void Stand() {
		animator.SetBool ("Run", false);
	}
	
	public void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.name == "RightWall") {
			touchingRightWall = true;
		} else if (collision.gameObject.name == "LeftWall") {
			touchingLeftWall = true;
		} else if (collision.gameObject.tag == "Floor") {
			touchingFloor = true;
			animator.SetBool ("Jumping", false);
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
			animator.SetBool ("Jumping", true);
			Vector2 vel = rigidbody2D.velocity;
			if (canJumpHigh) {
				vel.y += WorldController.playerJumpSpeed * 1.8f;
				push = WorldController.playerSpeed;
			} else {
				vel.y += WorldController.playerJumpSpeed;
			}
			rigidbody2D.velocity = vel;
		}
	}

	public void Hit() {
		if (canHitHard && obstacle) {
			animator.SetTrigger ("Hit");
			Destroy(obstacle.collider2D);
			camera.Shake();
		}
	}
}
