using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bull : MonoBehaviour
{
    public bool faceLeft = false;
    public float speed = 1;
    public int direction = 1;
    public Vector3 flyingSpeed = new Vector3(1, 1, 1);
    public AudioClip bullScream;
    public AudioClip hitSFX;
    public Vector2 pitchRange;

    private GameController gm;
    private Rigidbody2D rb;
    private bool isDead;
    private Vector3 currentAngle;
    private AudioSource localSource;

    private void Start() {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        localSource = gameObject.GetComponent<AudioSource>();
        isDead = false;
        currentAngle = new Vector3(0,0,0);
        if (faceLeft) {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            direction = -1;
        }
    }

    private void FixedUpdate() {
        if (!gm.isFightingABull() && !gm.isPlayerDead()) {
            rb.position = new Vector2(rb.position.x + speed * direction * Time.deltaTime, rb.position.y);
        }

        if (this.isDead) {
            transform.position = new Vector3(transform.position.x + flyingSpeed.x * (-direction), transform.position.y + flyingSpeed.y, transform.position.z);

            currentAngle = new Vector3(currentAngle.x, currentAngle.y, currentAngle.z + 10 * flyingSpeed.z);
            gameObject.transform.localRotation = Quaternion.Euler(currentAngle);

            float constraintSize = Mathf.Abs(gm.horizontalConstraints.y - gm.horizontalConstraints.x);
            if (transform.position.x <= -constraintSize + gm.horizontalConstraints.x || transform.position.x >= constraintSize + gm.horizontalConstraints.y) Destroy(gameObject);
        }
    }

    public void bullDied() {
        this.isDead = true;
        localSource.Stop();

        if (hitSFX != null) {
            localSource.PlayOneShot(hitSFX, 1);
        }
        if (bullScream != null) {
            localSource.clip = bullScream;
            localSource.loop = false;
            localSource.volume = 10;
            localSource.pitch = 1 + Random.Range(pitchRange.x, pitchRange.y);
            localSource.Play();
        }
        Destroy(gameObject.GetComponent<BoxCollider2D>());
    }
}
