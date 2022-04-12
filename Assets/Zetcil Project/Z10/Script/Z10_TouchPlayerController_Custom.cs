using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Z10_TouchPlayerController_Custom : MonoBehaviour
{
    [Header("Main Setting")]
    public Camera TargetCamera;
    public Transform TargetPlayer;
    public GameObject followPlayer;
    public float RaycastDistance = 10f;

    [Header("Collider Tag")]
    public string groundTag;
    public string worldButtonTag;

    [Header("Destination Setting")]
    public Transform Destination;
    public Vector3 Offset;

    [Header("Speed Setting")]
    public float playerSpeed = 1;

    [Header("Running Setting")]
    public bool isRun;
    public float playerWalkSpeed = 1;
    public float playerRunSpeed = 3;
    public TextMeshProUGUI runText;

    [Header("Animation Setting")]
    public float Distance;
    public float CompareDistance;
    public UnityEvent IdleAnimation;
    public UnityEvent WalkAnimation;
    public UnityEvent RunAnimation;

    #region --- Private Variables
    private static Vector2 beginTouchPosition;
    #endregion


    // Use this for initialization
    void Start() {
        Destination.position = TargetPlayer.position + Offset;
        if (isRun) {
            playerSpeed = playerRunSpeed;
            runText.text = "RUN";
        } else {
            playerSpeed = playerWalkSpeed;
            runText.text = "WALK";
        }
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
                        TargetPlayer.LookAt(Destination); // Make sure the target player always look at the destination
                    }
                }
            }
        }
    }

    void FixedUpdate() {
        if (Destination.position != Vector3.zero) {
            
            TargetPlayer.transform.position = Vector3.MoveTowards(TargetPlayer.transform.position, Destination.position, playerSpeed * Time.deltaTime);
            Distance = Vector3.Distance(TargetPlayer.transform.position, Destination.position);
            followPlayer.transform.position = new Vector3(TargetPlayer.position.x, followPlayer.transform.position.y, TargetPlayer.position.z);
            if (Distance > CompareDistance) {
                if (isRun) {
                    RunAnimation.Invoke();
                } else {
                    WalkAnimation.Invoke();
                }
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

    public void SwitchWalkRun() {
        isRun = !isRun;
        if (isRun) {
            playerSpeed = playerRunSpeed;
            if (runText is not null) runText.text = "RUN";
        } else {
            playerSpeed = playerWalkSpeed;
            if (runText is not null) runText.text = "WALK";
        }
    }
}
