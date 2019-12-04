using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Vector2 horizontalConstraints;
    public GameObject clockPrefab;
    public float reactionTime;
    public bool autoPlay = false;

    private MainCanvas mainCanvas;
    private GameObject mainCamera;
    private AudioListener mainAudio;
    private GameObject player;
    private GameObject collidedBull;
    private GameObject loserPanel;

    private bool frontCollided;
    private bool playerDead;
    private bool lost;
    private char reactionKey;    

    private float loserTimer;
    private float start;
    private int bullCount;

    void Start() {
        loserPanel = GameObject.Find("loserPanel");
        loserPanel.SetActive(false);

        mainCanvas = GameObject.FindWithTag("MainCanvas").GetComponent<MainCanvas>();
        player = GameObject.FindWithTag("Player");
        mainCamera = GameObject.FindWithTag("MainCamera");
        mainAudio = mainCamera.GetComponent<AudioListener>();

        frontCollided = false;
        playerDead = false;
        lost = false;
        loserTimer = 0;
        start = 0;
        bullCount = 0;
        mainCamera.transform.parent.GetComponent<CameraMovement>().setHorizontalConstraints(horizontalConstraints);
    }

    void Update() {
        if (frontCollided) {
            start += Time.deltaTime;

            bool pressedKey = Input.GetKeyDown(reactionKey.ToString());
            if (pressedKey && !playerDead) {
                attackBull();
                return;
            }

            bool pressedWrongKey = false;
            for (int i = 0; i < 26; i++) {
                if (Input.GetKeyDown(((char)(i + 97)).ToString())) {
                    pressedWrongKey = true;
                    break;
                }
            }


            if (start >= reactionTime * 1.1 || pressedWrongKey) {
                playerDied();
            }
        }
        if (lost) {
            loserTimer += Time.deltaTime;
            if (!loserPanel.active) loserPanel.SetActive(true);
            if (loserTimer >= 2.0f) {
                SceneManager.LoadScene("Menu");
            }
        }
    }

    public bool isFightingABull() { return frontCollided; }

    public void playerCollidedWithBull(GameObject bull) {
        frontCollided = true;
        start = 0;
        loserTimer = 0;
        collidedBull = bull;

        if (autoPlay) {
            attackBull();
            return;
        }

        mainCanvas.newClock(reactionTime);

        reactionKey = (char) Random.Range(97, 123);
        if (reactionKey == 'q') reactionKey = 'l';

        mainCanvas.newButton(reactionTime, reactionKey);
    }

    public void playerDied() {
        playerDead = true;
        mainCamera.transform.parent.gameObject.GetComponent<CameraMovement>().shouldFollow = false;
        mainCanvas.clearScreen();
        lost = true;
    }

    public bool isPlayerDead() {
        return playerDead;
    }

    public void stopPlayerAttack() {
        GameObject.FindWithTag("Player").GetComponent<Animator>().SetBool("attacking", false);
    }

    public void toggleAutoplay() {
        autoPlay = !autoPlay;
    }

    public bool isInsideHContraints(Vector2 p) {
        return p.x >= horizontalConstraints.x - 720 && p.x <= horizontalConstraints.y + 720; // camera size
    }

    private void attackBull() {
        player.GetComponent<Animator>().Play("player_attack");
        start = 0;
        loserTimer = 0;
        collidedBull.GetComponent<Bull>().bullDied();
        collidedBull = null;
        frontCollided = false;
        mainCanvas.clearScreen();
        mainCanvas.setBullCount(++bullCount);
    }
}
