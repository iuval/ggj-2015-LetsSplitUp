using UnityEngine;
using System.Collections;

public class ChannelingBar : MonoBehaviour {

	public Transform bar;
	public float max;

	public void Reset() {
		bar.transform.localScale = new Vector3(0, 1, 1);
	}
	
	public void SetValue(float value) {
		bar.transform.localScale = new Vector3(value / max, 1, 1);
	}

	public void Show() {
		gameObject.SetActive(true);
	}
	
	public void Hide() {
		gameObject.SetActive(false);
	}
}
