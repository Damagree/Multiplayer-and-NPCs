using UnityEngine;
//using Photon.Pun;
using UnityEngine.Events;

public class FpsMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] CharacterController controller;

    [SerializeField] float speed = 12f;
    [SerializeField] float gravity = -9.81f;

    [SerializeField] bool usingJump = false;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    [Header("Animation"), Space(10)]
    [Space(5)] public bool usingAnimation;
    [SerializeField] Animator targetAnimator;
    [SerializeField] bool usingWalkAnimation;
    [SerializeField] GameObject myMesh;

    //private PhotonView photonView;

    Vector3 velocity;
    bool isGrounded;

    //private void Awake() {
    //    photonView = GetComponent<PhotonView>();
    //    if (photonView.IsMine) {
    //        myMesh.SetActive(false);
    //    }
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (!photonView.IsMine)
        //    return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        
        if (usingJump) {
            if (Input.GetButtonDown("Jump") && isGrounded) {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (!usingAnimation)
            return;

        if (!usingWalkAnimation)
            return;

        if (x != 0 || z != 0) {
            targetAnimator.gameObject.GetComponent<AnimatorPlayer>().SetSpeed(true);
        } else {
            targetAnimator.gameObject.GetComponent<AnimatorPlayer>().SetSpeed(false);
        }
    }
}
