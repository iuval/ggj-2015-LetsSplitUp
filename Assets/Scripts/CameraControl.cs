using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent <Animator>();
	}

	public void Shake () {
		animator.SetTrigger ("Shake");
	}
}
