using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullSpawner : MonoBehaviour
{
    public bool shouldSpawn;
    public bool faceLeft;
    public float offsetInSeconds = 0;
    public float delayInSeconds = 1;
    public GameObject bullPrefab;

    private float timeFromLastSpawn = 0;
    private GameController gm;
    private bool firstSpawn = true;

    private void Start() {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        timeFromLastSpawn += Time.deltaTime;
        if (shouldSpawn && (timeFromLastSpawn > delayInSeconds + offsetInSeconds || firstSpawn)) {
            GameObject bull = GameObject.Instantiate(bullPrefab);
            bull.transform.position = transform.position;
            if (faceLeft) bull.GetComponent<Bull>().faceLeft = true;

            timeFromLastSpawn = offsetInSeconds;
            firstSpawn = false;
        }
    }
}
