using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public bool shouldFollow = true;
    public GameObject target;
    public Vector3 offset;

    private Vector2 horizontalConstraints;

    void Update()
    {
        if (shouldFollow) {
            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, target.transform.position.x + offset.x, 0.2f),
                Mathf.Lerp(transform.position.y, target.transform.position.y + offset.y, 0.2f),
                transform.position.z
            );

            if (horizontalConstraints.x > transform.position.x || horizontalConstraints == null) transform.position = new Vector3(horizontalConstraints.x, transform.position.y, transform.position.z);
            if (horizontalConstraints.y < transform.position.x || horizontalConstraints == null) transform.position = new Vector3(horizontalConstraints.y, transform.position.y, transform.position.z);
        }
    }

    public void setHorizontalConstraints (Vector2 hc) {
        horizontalConstraints = hc;
    }
}
