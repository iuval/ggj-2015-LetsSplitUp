using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Animator animator;
	private AudioSource stepAudio;
	private AudioSource hitAudio;
	private AudioSource jumpAudio;
	
	// 1 = can hit hard
	// 2 = can jump high
	public int power;

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
		stepAudio = GetComponents <AudioSource> ()[0];
		hitAudio = GetComponents <AudioSource> ()[1];
		jumpAudio = GetComponents <AudioSource> ()[2];
	}
	
	// Update is called once per frame
	void Update () {
		if (WorldController.playing) {
			Vector2 pos = gameObject.transform.position;
			pos.x += accel - WorldController.floorAccel + push;
			gameObject.transform.position = pos;
			push *= 0.8f;
		}
	}
	
	public void PlayJumpSFX(){
		jumpAudio.pitch = Random.Range (2f, 3f);
		jumpAudio.Play ();
	}

	public void PlayHitSFX(){
		hitAudio.pitch = Random.Range (0f, 3f);
		hitAudio.Play ();
	}

	public void PlayStepSFX(){
		stepAudio.pitch = Random.Range (2f, 3f);
		stepAudio.Play ();
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
		} else if (collision.gameObject.tag == "Spikes") {
//			KnockBack();
			animator.SetTrigger("Damage");
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
		if (CanHitJumpHigh()) {
			if (touchingFloor) {
				Vector2 vel = rigidbody2D.velocity;

				animator.SetBool ("Jumping", true);
				vel.y += WorldController.playerJumpSpeed * 1.5f;
				push = WorldController.playerSpeed * 2;

				rigidbody2D.velocity = vel;

				float dustX = transform.localScale.x == 1 ? collider2D.bounds.center.x - collider2D.bounds.size.x / 2 : collider2D.bounds.center.x + collider2D.bounds.size.x / 2;
				GameObject dust = (GameObject)GameObject.Instantiate(JumpDustPrefav, new Vector3(dustX, collider2D.bounds.center.y - collider2D.bounds.size.y / 3, 0), Quaternion.identity);
				dust.transform.localScale = transform.localScale;
			}
		} else {
			if (CanHitHard() && obstacle && obstacle.destroyable) {
				animator.SetTrigger ("Hit");
				obstacle.Hit(collider2D.bounds.center.y);
				obstacle = null;
				camera.Shake ();
			}
		}
	}

	public void ChangePowers() {
		GameObject.Instantiate (PoofPrefav, transform.position, Quaternion.identity);
		power = power == 0 ? 1 : 0;
		animator.SetInteger ("Power", power);

		BoxCollider2D box = GetComponent<BoxCollider2D> ();
		Vector3 size = box.size;
		if (CanHitHard()) {
			size /= 2;
		} else {
			size *= 2;
		}
		box.size = size;
	}

//	public void KnockBack() {
//		Vector2 velo = gameObject.rigidbody2D.velocity;
////		velo.x = -5.75f;
//		velo.y = WorldController.playerJumpSpeed * 1.15f;
//		gameObject.rigidbody2D.velocity = velo;
//	}

	public bool CanHitHard () { return power == 1; }
	public bool CanHitJumpHigh () { return power == 0; }
}
