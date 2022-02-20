using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractWithWorldButton : MonoBehaviour
{
    [Header("Gaze Setting")]
    [SerializeField] private float _maxDistanceInteraction = 10f; // max distance to interact
    [SerializeField] private string _interactableTag = "Interactable";

    [Header("Events Setting")]
    [SerializeField] private UnityEvent _onPointerEnter; // Event when pointer found desired gameobject
    [SerializeField] private UnityEvent _onPointerExit; // Event when pointer exit from current gameobject

    private GameObject _gazedAtObject = null; // Object that camera gazed at

    // Update is called once per frame
    void Update()
    {
        // casts ray towards camera's forward direction, to detect if a GameObject is being gazed at.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistanceInteraction))
        {
            // Make sure GameObject Tag name is correct.
            if (hit.transform.gameObject.tag == _interactableTag || (LayerMask.Equals(hit.transform.gameObject.layer, "UI") && hit.transform.gameObject.tag == _interactableTag))
            {
                // GameObject detected in front of the camera.
                if (_gazedAtObject != hit.transform.gameObject)
                {
                    _gazedAtObject = hit.transform.gameObject;// store new GameObject
                }
                // check if there's no object stored in gazedAtObject
                else if (_gazedAtObject != null)
                {
                    _onPointerEnter.Invoke();
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        _gazedAtObject?.GetComponent<Button>().onClick.Invoke(); // send message to object gazed at
                    }
                }
            }
            else
            {
                initGaze();
            }
        }
        // no object detected
        else
        {
            initGaze();
        }

    }

    // Initilize gaze so the value remain the same after interact
    private void initGaze()
    {
        _onPointerExit.Invoke();
        _gazedAtObject = null; // Set back to null gazedAtObject
    }
}
