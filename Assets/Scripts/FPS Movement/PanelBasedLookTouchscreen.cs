using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelBasedLookTouchscreen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

    public Vector2 playerVectorOutput;
    private int touchID;
    private Touch myTouch;

    private void Update() {
        foreach (Touch touch in Input.touches) {
            myTouch = touch;
            if (touch.fingerId == touchID) {
                if (touch.phase != TouchPhase.Moved) {
                    OutputVectorValue(Vector2.zero);
                }
            }
        }
    }

    private void OutputVectorValue(Vector2 outputValue) {
        playerVectorOutput = outputValue;
    }

    public Vector2 VectorOutput() {
        return playerVectorOutput;
    }

    public void OnDrag(PointerEventData eventData) {
        OutputVectorValue(new Vector2(eventData.delta.normalized.x, eventData.delta.normalized.y));
    }

    public void OnPointerDown(PointerEventData eventData) {
        OnDrag(eventData);
        touchID = eventData.pointerId;
        myTouch.fingerId = touchID;
    }

    public void OnPointerUp(PointerEventData eventData) {
        OutputVectorValue(Vector2.zero);
    }
}
