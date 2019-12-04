using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    public float delay;

    private bool fired = false;
    private float now = 0;

    private Animator anim;

    private void Start() {
        anim = gameObject.GetComponent<Animator>();
    }

    void Update() {
        if (fired) {
            now += Time.deltaTime;
            anim.SetFloat("time", now / delay);
            if (now >= delay) {
                Destroy(gameObject);
            }
        }
    }

    public void fire() {
        fired = true;
    }
}
