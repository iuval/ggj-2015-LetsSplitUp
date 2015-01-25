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

	public string floorName;

	public bool wantsToChange = false;

	private Obstacle obstacle;

	public CameraControl camera;
	public GameObject PoofPrefav;
	public GameObject JumpDustPrefav;

	// Use this for initialization
	void Start () {
		animator = GetComponent <Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 pos = gameObject.transform.position;
		pos.x += accel - WorldController.floorAccel + push;
		gameObject.transform.position = pos;
		push *= 0.8f;
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
		} else if (collision.gameObject.name == floorName) {
			touchingFloor = true;
			animator.SetBool ("Jumping", false);
		} else if (collision.gameObject.tag == "Obstacle") {
			obstacle = collision.gameObject.GetComponent<Obstacle>();
		}
	}
	
	public void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.name == "RightWall") {
			touchingRightWall = false;
		} else if (collision.gameObject.name == "LeftWall") {
			touchingLeftWall = false;
		} else if (collision.gameObject.name == floorName) {
			touchingFloor = false;
		} else if (collision.gameObject.tag == "Obstacle") {
			obstacle = null;
		}
	}

	public void Action() {
		if (canJumpHigh) {
			if (touchingFloor) {
				Vector2 vel = rigidbody2D.velocity;
				if (canJumpHigh) {
					animator.SetBool ("Jumping", true);
					vel.y += WorldController.playerJumpSpeed * 1.5f;
					push = WorldController.playerSpeed * 2;
				}
				rigidbody2D.velocity = vel;

				float dustX = transform.localScale.x == 1 ? collider2D.bounds.center.x - collider2D.bounds.size.x / 2 : collider2D.bounds.center.x + collider2D.bounds.size.x / 2;
				GameObject dust = (GameObject)GameObject.Instantiate(JumpDustPrefav, new Vector3(dustX, collider2D.bounds.center.y - collider2D.bounds.size.y / 3, 0), Quaternion.identity);
				dust.transform.localScale = transform.localScale;
			}
		} else {
			if (canHitHard && obstacle && obstacle.destroyable) {
				animator.SetTrigger ("Hit");
				obstacle.Destory();
				camera.Shake ();
			}
		}
	}

	public void ChangePowers() {
		GameObject.Instantiate (PoofPrefav, transform.position, Quaternion.identity);
		canHitHard = !canHitHard;
		canJumpHigh = !canJumpHigh;
		animator.SetTrigger ("Switch");

		BoxCollider2D box = GetComponent<BoxCollider2D> ();
		Vector3 size = box.size;
		if (canHitHard) {
			size /= 2;
		} else {
			size *= 2;
		}
		box.size = size;
	}
}
