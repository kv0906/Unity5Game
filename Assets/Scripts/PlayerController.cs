using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerController: MonoBehaviour {
	public int speedUpRemain;

	public bool isPause = false;
	private float moveSpeed;
	private float originalMoveSpeed;
	private int chargeSpeedUpTime;

	private SpeedUpBtnScript speedUpScript;
    private PauseBtnScript pauseScript;
	private Vector3 target;
	private Transform myTransform;
	private Animal animal;

	void Start () {
		animal = GetComponent<Animal> () as Animal;
		speedUpScript = GameObject.Find ("SpeedBtn").GetComponent<SpeedUpBtnScript>();
        pauseScript = GameObject.Find("PauseBtn").GetComponent<PauseBtnScript>();
		speedUpScript.ActivateSpeedUp ();

		myTransform = transform;
		target = transform.position;
		moveSpeed = GetComponent<Animal> ().moveSpeed;
		originalMoveSpeed = moveSpeed;
	}
		
	void Update () {
		if (isPause)
			return;
		
		if (Input.GetMouseButton (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
			if (hit) {
				if (hit.collider.gameObject.name.Equals ("SpeedBtn"))
					StartSpeedUp ();
				else if (hit.collider.gameObject.name.Equals ("PauseBtn")) { 
					pauseScript.Pause ();
					isPause = true;
				} else {
					target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					animal.MoveToNewTarget (target);
				}
			} 
			else {
				target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				animal.MoveToNewTarget (target);
			}
		}
		myTransform.position = Vector2.MoveTowards (myTransform.position, target, Time.deltaTime * moveSpeed);
	}
		
	public void StartSpeedUp() {
		if (chargeSpeedUpTime != 0)
			return;
		speedUpRemain = 5;
		moveSpeed += 0.5f;
		speedUpScript.DeactivateSpeedUp ();
		Invoke ("SpeedUpTimeCountDown", 2f);
	}

	void SpeedUpTimeCountDown() {
		speedUpRemain -= 1;
		if (speedUpRemain == 0) {
			moveSpeed = originalMoveSpeed;
			chargeSpeedUpTime = 30;
			Invoke ("ChargeSpeedUpCountDown", 1f);
			return;
		}
		Invoke ("SpeedUpTimeCountDown", 1f);
	}

	void ChargeSpeedUpCountDown() {
		chargeSpeedUpTime -= 1;
		if (chargeSpeedUpTime == 0) {
			speedUpScript.ActivateSpeedUp();
			return;
		}
		Invoke ("ChargeSpeedUpCountDown", 1f);
	}
}
