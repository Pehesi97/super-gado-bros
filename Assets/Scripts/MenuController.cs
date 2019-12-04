using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	private Button startButton;
	private AudioSource mainAudio;

	void Start () {
		mainAudio = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();

		startButton = GameObject.Find("startButton").GetComponent<Button>();
		startButton.onClick.AddListener(startGame);
	}

	void startGame () {
		mainAudio.Stop();
		SceneManager.LoadSceneAsync("Game");
	}
}
