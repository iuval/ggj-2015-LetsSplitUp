using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	private float accel = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SetAccel(float accel) {

	}

	public void MoveX(float speed) {
		Vector2 pos = gameObject.transform.position;
		pos.x += speed;
		gameObject.transform.position = pos;
	}
}
