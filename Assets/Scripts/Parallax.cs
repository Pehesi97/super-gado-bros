using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float level = 0.5f;
    public float speed = 0.5f;

    private GameObject camHolder;
    private GameController gm;
    void Start()
    {
        camHolder = GameObject.FindWithTag("MainCamera").transform.parent.gameObject;
        gm = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    private void FixedUpdate() {
        if (!gm.isPlayerDead()) {
            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, camHolder.transform.position.x * level, speed),
                transform.position.y,
                transform.position.z
            );
        }
    }
}
