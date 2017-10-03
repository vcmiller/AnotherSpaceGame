using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTarget : MonoBehaviour {
    new public Camera camera { get; private set; }
    public AudioListener listener { get; private set; }

    public Transform[] rotateCameras;

    private void Awake() {
        camera = GetComponent<Camera>();
        listener = GetComponent<AudioListener>();

        enabled = false;
    }

    private void OnEnable() {
        if (camera) {
            camera.enabled = true;
        }

        if (listener) {
            listener.enabled = true;
        }
    }

    private void OnDisable() {
        if (camera) {
            camera.enabled = false;
        }

        if (listener) {
            listener.enabled = false;
        }
    }

    private void LateUpdate() {
        foreach (Transform cam in rotateCameras) {
            if (cam) {
                cam.transform.rotation = transform.rotation;
            }
        }
    }
}
