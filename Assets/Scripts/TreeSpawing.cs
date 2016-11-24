using UnityEngine;
using System.Collections;

public class TreeSpawing : MonoBehaviour {
	public Sprite[] trees;

	void Start () {
		GetComponent<SpriteRenderer> ().sprite = trees [Random.Range (0, trees.Length)];
	}

}
