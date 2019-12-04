using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public float delay;

    private bool fired = false;
    private float now = 0;

    private Animator anim;

    private void Start() {
        anim = gameObject.GetComponent<Animator>();
    }

    private void Update() {
        if (fired) {
            now += Time.deltaTime;
            if (now >= delay) {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate() {
        if (this.anyButtonPressed()) anim.SetBool("pressed", true);
        else anim.SetBool("pressed", false);
    }

    public void fire() {
        fired = true;
    }

    private bool anyButtonPressed() {
        bool anythingPressed = false;
        for(int i=0; i<26; i++) {
            anythingPressed = anythingPressed || Input.GetKey(((char)(i+97)).ToString());
        }
        return anythingPressed;
    }
}
