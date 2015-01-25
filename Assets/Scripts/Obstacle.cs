using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	public bool destroyable;
	private Animator animator;

	void Start () {
		if (destroyable) {
			animator = GetComponent <Animator> ();
		}
	}

	void Update () {
	
	}

	public bool IsOutside() {
		return gameObject.transform.position.x < -WorldController.distanceToDestroyObstacle;
	}

	public void Destory () {
		animator.SetTrigger ("Destroy");
	}
}
