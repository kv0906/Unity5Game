using UnityEngine;
using System.Collections;

public class PredatorController : MonoBehaviour {
	public Transform playerTransform;
	public float chaseDuration;

	private float eyeSight;
	private ComputerController controller;
	private Transform myTransform;
	private float timeChasingRemain = 0;

	void Start () {
		myTransform = transform;
		controller = GetComponent<ComputerController> ();
		eyeSight = GetComponent<Animal> ().eyeSight;
	}

	void Update () {
		if (playerTransform != null) {
			if (Vector2.Distance (playerTransform.position, myTransform.position) < eyeSight) {
				if (timeChasingRemain > 0) {
					timeChasingRemain -= Time.deltaTime;
					if (timeChasingRemain <= 0)
						controller.timeRestingRemain = 3f;
				} 
				else 
					timeChasingRemain = chaseDuration;
				controller.target = playerTransform.position;
			}
		}
	}
}
