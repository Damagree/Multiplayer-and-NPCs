using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wolf3D.ReadyPlayerMe.AvatarSDK;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateAvatar : MonoBehaviourPunCallbacks {

    [SerializeField] private AvatarEntity avatarUrlsSaved;
    [SerializeField] private string AvatarURL = "https://d1a370nemizbjq.cloudfront.net/209a1bc2-efed-46c5-9dfd-edc8a1d9cbe4.glb";
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private GameObject playerBase;

    [Header("Turn ON/OFF Object When Done Setup"), Space(10)]
    [SerializeField] List<GameObject> turnOnWhenAvatarSetupDone;
    [SerializeField] List<GameObject> turnOffWhenAvatarSetupDone;
    [SerializeField] List<GameObject> DestroyWhenAvatarSetupDone;
    [SerializeField] List<GameObject> ForceDestroyWhenAvatarSetupDone;

    [SerializeField] bool isUsingPhoton = true;
    [SerializeField] new PhotonView photonView;

    private void Start() {
        if (isUsingPhoton) {
            if (ReferenceEquals(photonView, null)) {
                photonView = gameObject.GetComponentInParent<PhotonView>();
            }
        }

        LoadAvatar();
    }

    void LoadAvatar() {
        UpdateDebugText($"Get avatar's URL");

        //Debug.Log("Create Avatar Here!!!");
        foreach (Player player in PhotonNetwork.PlayerList) {
            if (player.CustomProperties.ContainsKey("playerAvatarUrl") && photonView.OwnerActorNr == player.ActorNumber) {
                AvatarURL = (string)player.CustomProperties["playerAvatarUrl"];
                break;
            }
        }

        if (!string.IsNullOrEmpty(avatarUrlsSaved.avatarUrl) && string.IsNullOrEmpty(AvatarURL)) {
            AvatarURL = avatarUrlsSaved.avatarUrl;
        }

        UpdateDebugText($"avatar url: {AvatarURL}");
        //Debug.Log($"avatar [{PhotonNetwork.LocalPlayer.ActorNumber}] url: {AvatarURL}");

        UpdateDebugText($"Started loading avatar...");
        AvatarLoader avatarLoader = new AvatarLoader();
        avatarLoader.LoadAvatar(AvatarURL, OnAvatarImported, OnAvatarLoaded);
    }

    private void OnAvatarImported(GameObject avatar) {
        UpdateDebugText($"Avatar imported. [{Time.timeSinceLevelLoad:F2}]");
    }

    private void OnAvatarLoaded(GameObject avatar, AvatarMetaData metaData) {
        if (isUsingPhoton) {
            AvatarLoadedWithPhoton(avatar, metaData);
        } else {
            AvatarLoaded(avatar, metaData);
        }
    }

    void AvatarLoadedWithPhoton(GameObject avatar, AvatarMetaData metaData) {
        

        UpdateDebugText($"Avatar loaded. [{Time.timeSinceLevelLoad:F2}]\n\n{metaData}");
        
        InitAvatar(true, avatar);
        OnAvatarSetupDone(true);

        UpdateDebugText("Controller Attached");
    }

    void OnAvatarSetupDone(bool isPhoton) {

        // Force Destroy either it's local or not
        foreach (GameObject gameObject in ForceDestroyWhenAvatarSetupDone) {
            if (!ReferenceEquals(gameObject, null)) {
                Destroy(gameObject);
            }
        }

        if (isPhoton) {
            if (!photonView.IsMine) {
                return;
            }
            foreach (GameObject gameObject in turnOnWhenAvatarSetupDone) {
                if (!ReferenceEquals(gameObject, null)) {
                    gameObject.SetActive(true);
                }
            }

            foreach (GameObject gameObject in turnOffWhenAvatarSetupDone) {
                if (!ReferenceEquals(gameObject, null)) {
                    gameObject.SetActive(false);
                }
            }
            foreach (GameObject gameObject in DestroyWhenAvatarSetupDone) {
                if (!ReferenceEquals(gameObject, null)) {
                    Destroy(gameObject);
                }
            }
        } 
        else {
            foreach (GameObject gameObject in turnOnWhenAvatarSetupDone) {
                if (!ReferenceEquals(gameObject, null)) {
                    gameObject.SetActive(true);
                }
            }

            foreach (GameObject gameObject in turnOffWhenAvatarSetupDone) {
                if (!ReferenceEquals(gameObject, null)) {
                    gameObject.SetActive(false);
                }
            }
            foreach (GameObject gameObject in DestroyWhenAvatarSetupDone) {
                if (!ReferenceEquals(gameObject, null)) {
                    Destroy(gameObject);
                }
            }
        }

        
    }

    void InitAvatar(bool isPhoton, GameObject avatar) {
        // Setting up avatar with player controller
        UpdateDebugText($"Setting up avatar");

        //set parent
        avatar.transform.SetParent(playerBase.transform);
        avatar.transform.localPosition = Vector3.zero;

        // set animator to playerBase animator to make photonAnimatorView Work Properly
        Animator animator = playerBase.GetComponent<Animator>();
        Animator createdAvatarAnimator = avatar.GetComponent<Animator>();

        animator.avatar = createdAvatarAnimator.avatar;
        animator.runtimeAnimatorController = createdAvatarAnimator.runtimeAnimatorController;
        createdAvatarAnimator.enabled = false;  // disable animator from the avatar created so we'll use main animator from the base

        // reference the animator to all that needed
        PlayerMovement playerMovement = playerBase.GetComponent<PlayerMovement>();
        AnimatorPlayer animatorPlayer = playerBase.GetComponent<AnimatorPlayer>();

        playerMovement.targetAnimator = animator;
        playerMovement.turnOffWhenFPP.Add(avatar);

        animatorPlayer.animator = animator;


        UpdateDebugText("Done setting up avatar");
        if (isPhoton) {
            UpdateDebugText("Initialize Controller...");

            // set up photon animator as parameter added
            PhotonAnimatorView photonAnimatorView = playerBase.GetComponent<PhotonAnimatorView>();
            photonAnimatorView.SetParameterSynchronized("Speed", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Discrete);

            // add photon animator view to photon view observeable component
            photonView.ObservedComponents.Add(photonAnimatorView);
        }
    }

    void AvatarLoaded(GameObject avatar, AvatarMetaData metaData) {
        UpdateDebugText($"Avatar loaded. [{Time.timeSinceLevelLoad:F2}]\n\n{metaData}");

        // Setting up avatar with player controller
        UpdateDebugText($"Setting up avatar");

        InitAvatar(false, avatar);
        OnAvatarSetupDone(false);

        UpdateDebugText("Controller Attached");
    }

    public void UpdateDebugText(string text) {
        if (isUsingPhoton) {
            if (photonView.IsMine) {
                debugText.text = text;
                return;
            }
        }
        debugText.text = text;
        
    }

    //public override void OnJoinedRoom() {
    //    base.OnJoinedRoom();
        
    //    LoadAvatar();
    //}

    //public override void OnPlayerEnteredRoom(Player newPlayer) {
    //    base.OnPlayerEnteredRoom(newPlayer);
    //    AvatarURL = (string)newPlayer.CustomProperties["playerAvatarUrl"];
    //}
}
