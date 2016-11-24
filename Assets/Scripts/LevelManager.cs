using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {
    public void LoadGame()
    {
        Application.LoadLevel(1);
		Time.timeScale = 1f;
    }
    public void LoadMenu()
    {
        Application.LoadLevel(0);
    }
}
