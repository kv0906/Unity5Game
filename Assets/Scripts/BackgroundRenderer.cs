using UnityEngine;
using System.Collections;

public class BackgroundRenderer : MonoBehaviour {
	public GameObject treeSpawning;
	public GameObject obstacleSpawning;

	void Awake() {
		float height = Camera.main.orthographicSize * 2;
		float width = height * Screen.width/ Screen.height; // basically height * screen aspect ratio

		SpriteRenderer renderer = GetComponent<SpriteRenderer> ();
		Sprite sprite = renderer.sprite;
		float unitWidth = sprite.textureRect.width / sprite.pixelsPerUnit;
		float unitHeight = sprite.textureRect.height / sprite.pixelsPerUnit;

		transform.localScale = new Vector2(width / unitWidth * 2, height / unitHeight * 2);

		// Spawn obstacles and trees
		Vector2 backgroundMax = renderer.bounds.max;
		Vector2 backgroundMin = renderer.bounds.min;
		for (int i = 0; i < 14; i++) {
			Vector2 randomPos = new Vector2 (Random.Range (backgroundMin.x, backgroundMax.x), 
				Random.Range (backgroundMin.y, backgroundMax.y));
			Instantiate (treeSpawning, randomPos, Quaternion.identity);
		}
		for (int i = 0; i < 10; i++) {
			Vector2 randomPos = new Vector2 (Random.Range (backgroundMin.x, backgroundMax.x), 
				Random.Range (backgroundMin.y, backgroundMax.y));
			Instantiate (obstacleSpawning, randomPos, Quaternion.identity);
		}
			
	}

}
