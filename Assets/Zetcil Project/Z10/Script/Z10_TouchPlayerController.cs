using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Z10_TouchPlayerController : MonoBehaviour
{
    [Header("Main Setting")]
    public Camera TargetCamera;
    public Transform TargetPlayer;

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

    // Use this for initialization
    void Start()
    {
        Destination.position = TargetPlayer.position + Offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTap())
        {
            RaycastHit hit;
            Ray ray = TargetCamera.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    Destination.position = hit.point + Offset;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (Destination.position != Vector3.zero)
        {
            TargetPlayer.transform.position = Vector3.MoveTowards(TargetPlayer.transform.position, Destination.position, playerSpeed * Time.deltaTime);
            Distance = Vector3.Distance(TargetPlayer.transform.position, Destination.position);
            if (Distance > CompareDistance)
            {
                WalkAnimation.Invoke();
            } else
            {
                IdleAnimation.Invoke();
            }
        }
    }

    public static bool IsTap()
    {
        bool result = false;
        float MaxTimeWait = 1;
        float VariancePosition = 1;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (Input.GetTouch(0).position.x < Screen.width / 2)
            {

                float DeltaTime = Input.GetTouch(0).deltaTime;
                float DeltaPositionLength = Input.GetTouch(0).deltaPosition.magnitude;

                if (DeltaTime > 0 && DeltaTime < MaxTimeWait && DeltaPositionLength < VariancePosition)
                    result = true;
            }
        }
        return result;
    }
}


