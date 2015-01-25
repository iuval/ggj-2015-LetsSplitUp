using UnityEngine;
using System.Collections;

public class CooldownClock : MonoBehaviour {
	
	private Animator animator;

	void Start () {
		animator = GetComponent <Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Show() {
		animator.SetTrigger ("Start");
	}
}
