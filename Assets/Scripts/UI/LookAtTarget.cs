using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    new Camera camera;

    private void Start() {
        Camera[] cameras = FindObjectsOfType<Camera>();
        if (cameras.Length == 1) {
            camera = cameras[0];
            return;
        }

        // if it's multiplayer
        //foreach (Camera item in cameras) {
        //    //TODO: check if current camera is this object
        //}
    }

    private void LateUpdate() {
        gameObject.transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
            camera.transform.rotation * Vector3.up);
    }

}
