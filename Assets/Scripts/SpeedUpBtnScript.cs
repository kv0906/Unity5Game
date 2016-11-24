using UnityEngine;
using System.Collections;

public class SpeedUpBtnScript : MonoBehaviour {
	public Sprite speedUpActive;
	public Sprite speedUpDeactive;

	void Start () {
		ActivateSpeedUp ();
	}
	
	public void ActivateSpeedUp() {
		GetComponent<SpriteRenderer> ().sprite = speedUpActive;
	}

	public void DeactivateSpeedUp() {
		GetComponent<SpriteRenderer> ().sprite = speedUpDeactive;
	}
}
