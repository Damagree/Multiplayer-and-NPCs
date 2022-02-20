using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerWithUnityEvent : MonoBehaviour {

    [SerializeField] string targetTag;

    [SerializeField, Space(10)] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag(targetTag))
            return;

        onTriggerEnter.Invoke();
    }

    private void OnTriggerExit(Collider other) {
        if (!other.gameObject.CompareTag(targetTag))
            return;

        onTriggerExit.Invoke();
    }
}
