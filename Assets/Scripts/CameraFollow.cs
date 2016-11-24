using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    private float xMax;
    private float yMax;
    private float xMin;
    private float yMin;
	private Transform myTransform;

    public Transform target;

	void Start () {
		myTransform = transform;
		SpriteRenderer renderer = GameObject.Find ("Background").GetComponent<SpriteRenderer>();
		xMax = renderer.bounds.max.x - Camera.main.aspect * Camera.main.orthographicSize;
		yMax = renderer.bounds.max.y - Camera.main.orthographicSize;
		xMin = renderer.bounds.min.x + Camera.main.aspect * Camera.main.orthographicSize;;
		yMin = renderer.bounds.min.y + Camera.main.orthographicSize;

		GameObject.Find ("SpeedBtn").transform.position = Camera.main.ViewportToWorldPoint (new Vector3 (0.1f, 0.1f, 10f));
		GameObject.Find ("PauseBtn").transform.position = Camera.main.ViewportToWorldPoint (new Vector3 (0.95f, 0.95f, 10f));
	}

	// Update is called once per frame
	void LateUpdate () {
		if (target)
        	myTransform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), 
				myTransform.position.z);
	}
}
