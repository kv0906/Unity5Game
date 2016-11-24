using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using Parse;

public class ScoreProcessing : MonoBehaviour {
	public GameObject saveNameDialog;
	private int score;
	private ParseObject lowestScoreId;

	IEnumerator Start () {
		score = MainController.totalScore;

		WWW www = new WWW("http://google.com");
		yield return www;
		if (www.isDone && www.bytesDownloaded > 0)
			StartCoroutine (FetchQuery ());
	}
		
	IEnumerator FetchQuery() {
		ParseQuery<ParseObject> query = ParseObject.GetQuery ("HighScore").WhereLessThan ("highscore", score).OrderBy("highscore");
		var fetchQuery = query.FindAsync ();
		while (!fetchQuery.IsCompleted)
			yield return null;
		
		IEnumerable<ParseObject> results = fetchQuery.Result;
		List<ParseObject> resultList = results.ToList();
		if (resultList == null || resultList.Count == 0) 
			return false;

		lowestScoreId = resultList.First();
		saveNameDialog.SetActive(true);
	}
		
	public void HideNameDialog() {
		saveNameDialog.SetActive(false);
	}

	public void SubmitScore() {
		InputField inputField = transform.Find ("nameDialog").Find ("InputField").gameObject.GetComponent<InputField>();
		string name = inputField.text;
		if (name.Length == 0)
			return;

		HideNameDialog (); 

		lowestScoreId ["username"] = name;
		lowestScoreId ["highscore"] = score;
		lowestScoreId.SaveAsync ();
	}
}
