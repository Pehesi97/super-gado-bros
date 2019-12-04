using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed;
    public float runningSpeed;
    public float deadZone;
    public Vector3 flyingSpeed = new Vector3(20, 2, 10);

    private GameController gm;
    private Rigidbody2D rb;
    private SpriteRenderer srender;
    private Animator animator;
    private CameraShake shaker;

    private Vector3 currentAngle;
    private int deathDirection;

    void Start () {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        srender = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        shaker = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        currentAngle = new Vector3(0, 0, 0);
    }

    void Update () {
        if (!gm.isFightingABull() && !gm.isPlayerDead()) {
            if (Input.GetButtonDown("Autoplay")) gm.toggleAutoplay();

            float hAxis = 0;
            bool running = false;
            if (gm.autoPlay) {
                hAxis = this.getAxis();
                running = true;
            } else {
                hAxis = Input.GetAxisRaw("Horizontal");
                running = Input.GetButton("Run");
            }

            if (shaker) shaker.isShaking = false;
            animator.SetBool("running", running);

            if (!this.isIdle(hAxis) && !animator.GetCurrentAnimatorStateInfo(0).IsName("player_attack")) {
                if (shaker && running) shaker.isShaking = true;
                this.setSpriteDirection(hAxis);
                
                animator.SetBool("walking", true);
                Vector2 newPosition = new Vector2(
                    rb.position.x + (running ? runningSpeed * Mathf.Sign(hAxis) : walkingSpeed * hAxis)*Time.deltaTime,
                    rb.position.y
                );

                if (gm.isInsideHContraints(newPosition))
                    rb.position = newPosition;
            } else {
                animator.SetBool("running", false);
                animator.SetBool("walking", false);
            }
        }

        if (gm.isPlayerDead()) {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            Destroy(gameObject.GetComponent<Collider2D>());
            transform.position = new Vector3(transform.position.x + this.flyingSpeed.x * this.deathDirection, transform.position.y + this.flyingSpeed.y, transform.position.z);

            currentAngle = new Vector3(currentAngle.x, currentAngle.y, currentAngle.z + this.flyingSpeed.z);
            gameObject.transform.localRotation = Quaternion.Euler(currentAngle);

            if (transform.position.y > 2000) Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Enemy") {
            this.deathDirection = coll.gameObject.transform.position.x < gameObject.transform.position.x ? 1 : -1;

            if (!gm.isPlayerDead()) {
                if (Input.GetButton("Run") && isPlayerFrontColliding(coll.gameObject) || gm.autoPlay) {
                    gm.playerCollidedWithBull(coll.gameObject);
                } else {
                    this.playerDied();
                }
            }
        }
    }

    private bool isPlayerFrontColliding(GameObject other) {
        return (srender.flipX && other.transform.position.x < gameObject.transform.position.x)
                || (!srender.flipX && other.transform.position.x > gameObject.transform.position.x);
    }

    private void setSpriteDirection (float hAxis) {
        if (hAxis < -deadZone) srender.flipX = true;
        else if (hAxis > deadZone) srender.flipX = false;
    }

    private bool isIdle (float hAxis) {
        return hAxis >= -deadZone && hAxis <= deadZone;
    }

    private void playerDied() {
        gm.playerDied();
    }

    private int getAxis() {
        GameObject closestBull = null;
        foreach (GameObject bull in GameObject.FindGameObjectsWithTag("Enemy")) {
            if (gm.isInsideHContraints(bull.transform.position)) {
                if (closestBull) {
                    float currentDistance = bull.transform.position.x - transform.position.x;
                    float distanceSaved = closestBull.transform.position.x - transform.position.x;

                    if (currentDistance * currentDistance < distanceSaved * distanceSaved) closestBull = bull;
                } else {
                    closestBull = bull;
                }
            }
        }
        return closestBull == null
            ? 0
            : closestBull.transform.position.x > transform.position.x ? 1 : -1;
    }
}
