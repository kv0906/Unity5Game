using UnityEngine;
using System.Collections;

public class PauseBtnScript : MonoBehaviour {

    public GameObject pauseMenu;
	public GameObject eventSystem;

    public void Pause()
    {
		eventSystem.SetActive (true);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
		eventSystem.SetActive (false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
		GameObject.Find ("player").GetComponent<PlayerController> ().isPause = false;
    }
    public void LoadMenu()
    {
        Application.LoadLevel(0);
    }
}
