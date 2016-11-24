using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Animal: MonoBehaviour {
	const int STATE_IDLE = 0;
	const int STATE_WALK = 1;
	const int STATE_ATTACK = 2;
	const int STATE_BEING_KILLED = 3;
	const int LEFT = 0;
	const int RIGHT = 1;

	public int current_direction;
	private int current_state = 0;
	private int previous_state;

	public GameObject[] preys;
	public GameObject[] predators;
	public float moveSpeed;
	public float moveRadius;
	public float minWaitTime; 
	public float maxWaitTime; 
	public float eyeSight;
	private Vector3 target;

	private MainController mainController;
	private Animator animator;
	private Rigidbody2D myRigidBody2D;
	private CircleCollider2D myCircleCollider2D;
	
	public AudioClip clip;

	void Start() {
		target = transform.position;

		mainController = GameObject.Find ("Main Controller").GetComponent<MainController> ();

		myRigidBody2D = gameObject.AddComponent<Rigidbody2D> ();
		myRigidBody2D.gravityScale = 0;

		myCircleCollider2D = gameObject.AddComponent<CircleCollider2D> ();
		myCircleCollider2D.radius = 0.2f;
		animator = GetComponent<Animator> () as Animator;
	}

	void Update() {
		if (Vector2.SqrMagnitude (transform.position - target) > 0.0001 && current_state == STATE_IDLE) {
			current_state = STATE_WALK;
			animator.SetInteger ("state", STATE_WALK);
		}
		
		else if (Vector2.SqrMagnitude(transform.position - target) < 0.0001 && current_state == STATE_WALK) {
			current_state = STATE_IDLE;
			animator.SetInteger ("state", STATE_IDLE);
		}
	}

	public void MoveToNewTarget(Vector3 target) {
		this.target = target;
		if (target.x < transform.position.x && current_direction != LEFT) {
			transform.Rotate (0, -180, 0);
			current_direction = LEFT;
		} 
		else if (target.x > transform.position.x && current_direction != RIGHT) {
			transform.Rotate (0, 180, 0);
			current_direction = RIGHT;
		}
	}
		
	void OnCollisionEnter2D(Collision2D other) {
		if (isPredator (other.gameObject.tag)) {
			other.gameObject.GetComponent<Animal> ().AttackAnimate ();
			current_state = STATE_BEING_KILLED;
			mainController.AnimalDie (other.gameObject.name, gameObject, clip);
			animator.SetInteger ("state", STATE_BEING_KILLED);
		}
	}

	public void AttackAnimate() {
		if (current_state != STATE_ATTACK)
			previous_state = current_state;
		current_state = STATE_ATTACK;
		animator.SetInteger ("state", STATE_ATTACK);
	}

	public void AfterAttackAnimate() {
		current_state = previous_state;
		animator.SetInteger ("state", current_state);
	}

	void OnBeingKilled() {
		Destroy(gameObject); 
	}


	bool isPredator(string tag) {
		foreach (GameObject predator in predators) {
			if (predator.name.Equals (tag))
				return true;
		}
		return false;
	}

}
