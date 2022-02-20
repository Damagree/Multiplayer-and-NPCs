using UnityEngine;
using Photon.Pun;

public class MouseLook : MonoBehaviour
{

    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] Transform playerBody;

    [Header("Camera")]
    [SerializeField] new Camera camera;
    [SerializeField] float defaultFOV;

    private PhotonView photonView;

    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponentInParent<PhotonView>();
        if (!photonView.IsMine) {
            Destroy(gameObject);
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Zoom Control
        float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScrollWheel > 0)
        {
            camera.fieldOfView--;
        }
        else if (mouseScrollWheel < 0)
        {
            camera.fieldOfView++;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            camera.fieldOfView = defaultFOV;
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
