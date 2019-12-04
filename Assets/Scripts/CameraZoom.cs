using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public List<int> angles;
    public float lerpFactor = 0.5f;
    public float zoom;

    private GameController gm;
    private Vector3 currentAngle;
    private int randomAngle;
    private Camera mainCamera;
    private float originalSize;

    private void Start() {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        currentAngle = new Vector3(0, 0, 0);
        mainCamera = gameObject.GetComponent<Camera>();
        originalSize = 50;
    }

    // Update is called once per frame
    private void Update() {
        if(gm.isFightingABull()) {
            currentAngle = Vector3.Lerp(
                currentAngle,
                new Vector3(0, 0, this.randomAngle),
                this.lerpFactor * Time.deltaTime
            );
            gameObject.transform.localRotation = Quaternion.Euler(currentAngle);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoom, lerpFactor*Time.deltaTime);
        } else {
            randomAngle = angles[(int) Random.Range(0, angles.Count)];

            currentAngle = Vector3.Lerp(
                currentAngle,
                new Vector3(0, 0, 0),
                2 * this.lerpFactor * Time.deltaTime
            );
            gameObject.transform.localRotation = Quaternion.Euler(currentAngle);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, originalSize, lerpFactor*Time.deltaTime);
        }
    }
}
