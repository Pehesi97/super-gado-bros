using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool isShaking;
    public Vector2 speed;

    private int angle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isShaking) {
            transform.localPosition = new Vector3(
                Mathf.Sin(angle*(Mathf.PI/180f)*speed.x),
                Mathf.Cos(angle*(Mathf.PI/180f)*speed.y),
                transform.localPosition.z
            );

            angle = angle >= 360 ? 0 : angle + 1;
        }
    }
}
