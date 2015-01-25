using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	private Animator animator;

	void Start () {
		animator = GetComponent <Animator>();
	}
	
	public void Show() {
		animator.SetTrigger ("Show");
	}

	public void Hide() {
		animator.SetTrigger ("Hide");
	}

	public void Disable() {
		gameObject.SetActive (false);
	}
}
