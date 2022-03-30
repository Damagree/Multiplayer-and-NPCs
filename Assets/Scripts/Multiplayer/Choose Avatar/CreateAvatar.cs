using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wolf3D.ReadyPlayerMe.AvatarSDK;
using TMPro;

public class CreateAvatar : MonoBehaviour
{
    [SerializeField] private AvatarUrls avatarUrlsSaved;
    [SerializeField] private string AvatarURL = "https://d1a370nemizbjq.cloudfront.net/209a1bc2-efed-46c5-9dfd-edc8a1d9cbe4.glb";
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private GameObject playerBase;

    [Header("Turn ON/OFF Object When Done Setup"), Space(10)]
    [SerializeField] List<GameObject> turnOnWhenAvatarSetupDone;
    [SerializeField] List<GameObject> turnOffWhenAvatarSetupDone;

    private void Start() {
        UpdateDebugText($"Get avatar's URL");
        if (!string.IsNullOrEmpty(avatarUrlsSaved.avatarUrl)) {
            AvatarURL = avatarUrlsSaved.avatarUrl;
        }
        UpdateDebugText($"avatar url: {AvatarURL}");

        UpdateDebugText($"Started loading avatar...");
        AvatarLoader avatarLoader = new AvatarLoader();
        avatarLoader.LoadAvatar(AvatarURL, OnAvatarImported, OnAvatarLoaded);
    }

    private void OnAvatarImported(GameObject avatar) {
        UpdateDebugText($"Avatar imported. [{Time.timeSinceLevelLoad:F2}]");
    }

    private void OnAvatarLoaded(GameObject avatar, AvatarMetaData metaData) {
        UpdateDebugText($"Avatar loaded. [{Time.timeSinceLevelLoad:F2}]\n\n{metaData}");
        
        // Setting up avatar with player controller
        UpdateDebugText($"Setting up avatar");
        
        //set parent
        avatar.transform.SetParent(playerBase.transform);
        avatar.transform.localPosition = Vector3.zero;

        // set animator
        Animator animator = avatar.GetComponent<Animator>();
        PlayerMovement playerMovement = playerBase.GetComponent<PlayerMovement>();
        AnimatorPlayer animatorPlayer = playerBase.GetComponent<AnimatorPlayer>();

        playerMovement.targetAnimator = animator;
        playerMovement.turnOffWhenFPP.Add(avatar);

        animatorPlayer.animator = animator;

        UpdateDebugText("Done setting up avatar");

        UpdateDebugText("Initialize Controller...");
        foreach (GameObject gameObject in turnOnWhenAvatarSetupDone) {
            gameObject.SetActive(true);
        }

        foreach (GameObject gameObject in turnOffWhenAvatarSetupDone) {
            gameObject.SetActive(false);
        }

        UpdateDebugText("Controller Attached");
    }

    public void UpdateDebugText(string text) {
        debugText.text = text;
    }
}
