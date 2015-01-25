using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	public bool destroyable;
	private Animator animator;

	public GameObject explotionPrefav;

	void Start () {
		if (destroyable) {
			animator = GetComponent <Animator> ();
		}
	}

	public bool IsOutside() {
		return gameObject.transform.position.x < -WorldController.distanceToDestroyObstacle;
	}

	public void Hit (float height) {
		GameObject.Instantiate (explotionPrefav, new Vector3(collider2D.bounds.center.x, height, 0), Quaternion.identity);
		animator.SetTrigger ("Destroy");
	}
}
