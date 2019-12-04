using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private Button startButton;

    void Start () {
		startButton = GameObject.Find("startButton").GetComponent<Button>();
		startButton.onClick.AddListener(startGame);
	}

	void startGame () {
		SceneManager.LoadSceneAsync("Game");
	}
}
