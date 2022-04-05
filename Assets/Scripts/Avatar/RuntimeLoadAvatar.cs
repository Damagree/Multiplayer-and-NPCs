using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wolf3D.ReadyPlayerMe.AvatarSDK;
using TMPro;

public class RuntimeLoadAvatar : MonoBehaviour
{
    public GameObject avatar;

    [SerializeField] GameObject previewLock;
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] Button playBtn;
    [SerializeField] AvatarEntity avatarEntity;
    public Transform spawnLoadedAvatar;


    // Start is called before the first frame update
    void Start()
    {
        string url = avatarEntity.avatarUrl;
        AvatarLoader avatarLoader = new AvatarLoader();
        avatarLoader.LoadAvatar(url, null, OnAvatarImported);
    }

    private void OnAvatarImported(GameObject avatar, AvatarMetaData metaData) {
        UpdateDebugText($"Avatar Loaded!");
        UpdateDebugText($"Setting up preview avatar");
        this.avatar = avatar;

        avatar.transform.SetPositionAndRotation(spawnLoadedAvatar.position, spawnLoadedAvatar.rotation);

        UpdateDebugText($"Successfully setup preview avatar");
    }

    public void UpdateDebugText(string text) {
        infoText.text = text;
    }
}
