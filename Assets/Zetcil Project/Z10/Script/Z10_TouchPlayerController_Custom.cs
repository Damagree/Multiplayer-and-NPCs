using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Z10_TouchPlayerController_Custom : MonoBehaviour
{
    [Header("Main Setting")]
    public Camera TargetCamera;
    public Transform TargetPlayer;
    public float RaycastDistance = 10f;

    [Header("Collider Tag")]
    public string groundTag;
    public string worldButtonTag;

    [Header("Destination Setting")]
    public Transform Destination;
    public Vector3 Offset;

    [Header("Speed Setting")]
    public float playerSpeed = 1;

    [Header("Animation Setting")]
    public float Distance;
    public float CompareDistance;
    public UnityEvent IdleAnimation;
    public UnityEvent WalkAnimation;

    #region --- Private Variables
    private static Vector2 beginTouchPosition;
    #endregion


    // Use this for initialization
    void Start() {
        Destination.position = TargetPlayer.position + Offset;
    }

    // Update is called once per frame
    void Update() {
        if (IsTap()) {
            // Single Raycast Hit
            //RaycastHit hit;
            //Ray ray = TargetCamera.ScreenPointToRay(Input.GetTouch(0).position);

            //if (Physics.Raycast(ray, out hit)) {
            //    if (hit.collider != null) {
            //        if (hit.collider.CompareTag(groundTag)) {
            //            Destination.position = hit.point + Offset;
            //        }
            //    }
            //}

            // Multiple Raycast Hit
            RaycastHit[] hits;
            Ray ray = TargetCamera.ScreenPointToRay(Input.GetTouch(0).position);
            hits = Physics.RaycastAll(ray, RaycastDistance);
            Array.Sort(hits, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance)); // sort hits order

            if (hits.Length > 0) {
                for (int i = 0; i < hits.Length; i++) {
                    if (hits[i].collider.CompareTag(worldButtonTag)) { // check if it hit World Button
                        return;
                    }
                    if (hits[i].collider != null && hits[i].collider.CompareTag(groundTag)) {
                        Destination.position = hits[i].point + Offset;
                    }
                }
            }
        }
    }

    void FixedUpdate() {
        if (Destination.position != Vector3.zero) {
            TargetPlayer.LookAt(Destination); // Make sure the target player always look at the destination
            TargetPlayer.transform.position = Vector3.MoveTowards(TargetPlayer.transform.position, Destination.position, playerSpeed * Time.deltaTime);
            Distance = Vector3.Distance(TargetPlayer.transform.position, Destination.position);
            if (Distance > CompareDistance) {
                WalkAnimation.Invoke();
            } else {
                IdleAnimation.Invoke();
            }
        }
    }

    public static bool IsTap() {
        bool result = false;
        float MaxTimeWait = 1;
        float VariancePosition = 1;

        if (Input.touchCount == 1 ) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                beginTouchPosition = touch.position;
            }
             
            if (touch.phase == TouchPhase.Ended && touch.position == beginTouchPosition) {
                //// Left side touch
                //if (Input.GetTouch(0).position.x < Screen.width / 2) {

                //    float DeltaTime = Input.GetTouch(0).deltaTime;
                //    float DeltaPositionLength = Input.GetTouch(0).deltaPosition.magnitude;

                //    if (DeltaTime > 0 && DeltaTime < MaxTimeWait && DeltaPositionLength < VariancePosition)
                //        result = true;
                //}
                
                //// Full Screen Touch Possible
                float DeltaTime = Input.GetTouch(0).deltaTime;
                float DeltaPositionLength = Input.GetTouch(0).deltaPosition.magnitude;

                if (DeltaTime > 0 && DeltaTime < MaxTimeWait && DeltaPositionLength < VariancePosition)
                    result = true;
            }
        }
        return result;
    }
}
