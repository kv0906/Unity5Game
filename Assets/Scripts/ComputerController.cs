using UnityEngine;
using System.Collections;

public class ComputerController : MonoBehaviour {
	public Vector3 target;
	public float timeRestingRemain = 0;

	private Animal animal;
	private Vector3 randomTarget;
	private Vector2 backgroundMax; 
	private Vector2 backgroundMin;

	void Start () {
		animal = GetComponent<Animal> ();
		target = transform.position;

		SpriteRenderer background = GameObject.Find ("Background").GetComponent<SpriteRenderer>();
		backgroundMax = background.bounds.max;
		backgroundMin = background.bounds.min;
		Invoke ("Wander", animal.minWaitTime);
	}

	void Update () {
		if (timeRestingRemain <= 0)
			transform.position = Vector2.MoveTowards (transform.position, target, Time.deltaTime * animal.moveSpeed);
		else
			timeRestingRemain -= Time.deltaTime;
	}

	void Wander() {
		randomTarget = new Vector3 (Random.Range (-animal.moveRadius, animal.moveRadius) + transform.position.x, 
			Random.Range (-animal.moveRadius, animal.moveRadius) + transform.position.y, transform.position.z);
		if (randomTarget.x < backgroundMin.x || randomTarget.x > backgroundMax.x ||
		    randomTarget.y < backgroundMin.y || randomTarget.y > backgroundMax.y) 
			Wander ();
		else {
			target = randomTarget;
			animal.MoveToNewTarget (randomTarget);
			Invoke ("Wander", Random.Range (animal.minWaitTime, animal.maxWaitTime));
		}

	}
}
