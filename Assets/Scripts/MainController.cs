using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainController : MonoBehaviour {
	public GameObject[] playableSprites;

	private const string PLAYER_NAME = "player";
	private GameObject[] preyPrefabs;
	private GameObject[] predatorPrefabs;
	private GameObject playerPrefab;
	private GameObject player;
	private float chaseDuration = 7f;

	private static Vector2 backgroundMax; 
	private static Vector2 backgroundMin;
	public float rateOfPrey = 0.8f;
	private float minSpawnTime = 1f; 
	private float maxSpawnTime = 3f; 

	private const int NUM_ANIMAL_LIMIT = 50;
	private const int NUM_PREDATOR_LIMIT = 40;
	private const float RATE_OF_SAME_KIND = 0.1f;
	private int numOfAnimal = 0;
	private int numOfPredator = 0;

	private const int PASS_STAGE_POINT = 10;
	public static int totalScore = 0;
	private int stageScore;

	private GUIText scoreGUI;

	public AudioClip[] soundtracks;
	public AudioSource audioSource;

	void Start () {
		ChooseAnimalForPlayer ();
	
		totalScore = 0;
		scoreGUI = gameObject.AddComponent<GUIText> ();
		scoreGUI.fontSize = 24;
		scoreGUI.fontStyle = FontStyle.Bold;
		scoreGUI.transform.position = new Vector2 (0f, 1f);
		scoreGUI.text = totalScore.ToString () + "\n" + stageScore.ToString() + "/10";

		SpriteRenderer background = GameObject.Find ("Background").GetComponent<SpriteRenderer>();
		backgroundMax = background.bounds.max;
		backgroundMin = background.bounds.min;
		PlayNextSong ();

		Invoke("ChooseLocationToSpawn", minSpawnTime);
	}
		
	void ChooseLocationToSpawn() {
		if (numOfAnimal == NUM_ANIMAL_LIMIT)
			return;

		Vector2 randomPos = new Vector2 (Random.Range (backgroundMin.x, backgroundMax.x), 
			                    Random.Range (backgroundMin.y, backgroundMax.y));
		Vector2 randomPortView = Camera.main.WorldToViewportPoint (randomPos);
		if ((randomPortView.x < 1f && randomPortView.x > 0) && (randomPortView.y < 1f && randomPortView.y > 0))
			ChooseLocationToSpawn ();
		else {
			ChooseAnimalToSpawn (randomPos);
			numOfAnimal += 1;
			Invoke ("ChooseLocationToSpawn", Random.Range (minSpawnTime, maxSpawnTime));
		}
	}

	void ChooseAnimalToSpawn(Vector2 spawnPos) {
		float randomNum = Random.Range (0f, 1f);
		GameObject animal;
		if (randomNum <= RATE_OF_SAME_KIND) 
			animal = Instantiate (playerPrefab, spawnPos, Quaternion.identity) as GameObject;

		else if (randomNum <= rateOfPrey || predatorPrefabs.Length == 0) 
			animal = Instantiate (preyPrefabs [Random.Range (0, preyPrefabs.Length)], spawnPos, Quaternion.identity) as GameObject;

		else {
			if (numOfPredator == NUM_PREDATOR_LIMIT) {
				ChooseAnimalToSpawn (spawnPos);
				return;
			}
			animal = Instantiate (predatorPrefabs [Random.Range (0, predatorPrefabs.Length)], spawnPos, Quaternion.identity) as GameObject;
			PredatorController controller = animal.AddComponent<PredatorController>();
			controller.playerTransform = player.transform;
			controller.chaseDuration = chaseDuration;
			numOfPredator += 1;
		}
		animal.AddComponent<ComputerController>();
		animal.transform.parent = transform;

		if (animal.gameObject.tag.Equals("hydra"))
			predatorPrefabs = new GameObject[0];
	}

	void ChooseAnimalForPlayer() {
		playerPrefab = playableSprites [Random.Range (0, playableSprites.Length)];
		player = Instantiate (playerPrefab, Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f)), 
			Quaternion.identity) as GameObject;
		player.name = PLAYER_NAME;
		player.transform.parent = transform;
		player.AddComponent <PlayerController>();
		preyPrefabs = player.GetComponent<Animal> ().preys;
		predatorPrefabs = player.GetComponent<Animal> ().predators;
		Camera.main.GetComponent<CameraFollow> ().target = player.transform;
	}

	public void AnimalDie(string killer, GameObject beingKilled, AudioClip clip) {
		audioSource.PlayOneShot(clip, 1);
		
		bool isSpawningFull = numOfAnimal == NUM_ANIMAL_LIMIT;
		if (killer.Equals (player.name)) {
			stageScore += 1;
			totalScore += 1;

			if (stageScore == PASS_STAGE_POINT) {
				foreach (Transform animal in transform) 
					Destroy (animal.gameObject);
				ChooseAnimalForPlayer ();
				stageScore = 0;
				rateOfPrey *= 90f / 100;
			}
			scoreGUI.text = totalScore.ToString () + "\n" + stageScore.ToString() + "/10";
		}

		else if (beingKilled.name.Equals(player.name)) {
			Application.LoadLevel (2);
			return;
		}

		else if (isPredator (beingKilled.tag))
			numOfPredator -= 1;
		
		numOfAnimal -= 1;
		if (isSpawningFull)
			ChooseLocationToSpawn ();
	}

	bool isPredator(string tag) {
		foreach (GameObject predator in predatorPrefabs)
			if (predator.name.Equals(tag))
				return true;
		return false;
	}

	void PlayNextSong() {
		audioSource.clip = soundtracks[Random.Range(0, soundtracks.Length)];
		audioSource.Play();
		Invoke("PlayNextSong", audioSource.clip.length);
	}
			
}
