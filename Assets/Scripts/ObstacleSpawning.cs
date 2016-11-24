using UnityEngine;
using System.Collections;

public class ObstacleSpawning : MonoBehaviour {
	public Sprite[] obstacles;

	void Start () {
		GetComponent<SpriteRenderer> ().sprite = obstacles [Random.Range (0, obstacles.Length)];
	}
}
