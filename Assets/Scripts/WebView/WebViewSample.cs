using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wolf3D.ReadyPlayerMe.AvatarSDK;
using Photon.Pun;
using TMPro;

public class WebViewSample : MonoBehaviour {

    public GameObject avatar;
    public Transform spawnLoadedAvatar;
    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    [SerializeField] private WebView webView;
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private GameObject loadingLabel = null;
    [SerializeField] private Button displayButton = null;
    [SerializeField] private Button closeButton = null;

    [SerializeField] AvatarEntity avatarEntity;
    [SerializeField] private UnityEvent onAvatarCreatedEvent;

    private void Start() {
        displayButton.onClick.AddListener(DisplayWebView);
        closeButton.onClick.AddListener(HideWebView);
        playerProperties["playerAvatarUrl"] = "";
#if UNITY_EDITOR
        displayButton.interactable = true;
        CustomPropertiesDebug();
#endif
    }

    // Display WebView or create it if not initialized yet 
    private void DisplayWebView() {
        if (webView == null) {
            webView = FindObjectOfType<WebView>();
        }

        if (webView.Loaded) {
            webView.SetVisible(true);
        } else {
            webView.CreateWebView();
            webView.OnAvatarCreated = OnAvatarCreated;
        }

        closeButton.gameObject.SetActive(true);
        displayButton.gameObject.SetActive(false);
    }

    private void HideWebView() {
        webView.SetVisible(false);
        closeButton.gameObject.SetActive(false);
        displayButton.gameObject.SetActive(true);
    }

    // WebView callback for retrieving avatar url
    private void OnAvatarCreated(string url) {
        if (avatar) Destroy(avatar);

        UpdateDebugText($"Avatar Created");

        webView.SetVisible(false);
        loadingLabel.SetActive(true);
        displayButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);

        UpdateDebugText($"Storing url on server cache");
        avatarEntity.avatarUrl = url.ToString(); // save the urls to scriptableObject

        UpdateDebugText($"Storing url on server cache");
        playerProperties["playerAvatarUrl"] = url.ToString(); // set customProperties to the photon server
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);

        UpdateDebugText($"Started Downlading avatar...");
        AvatarLoader avatarLoader = new AvatarLoader();
        avatarLoader.LoadAvatar(url, null, OnAvatarImported);
    }

    // AvatarLoader callback for retrieving loaded avatar game object
    private void OnAvatarImported(GameObject avatar, AvatarMetaData metaData) {
        UpdateDebugText($"Avatar Loaded!");
        UpdateDebugText($"Setting up preview avatar");
        this.avatar = avatar;
        avatarEntity.avatar = avatar; // Try to save game object to the scriptableObject

        loadingLabel.SetActive(false);
        displayButton.gameObject.SetActive(true);

        avatar.transform.SetPositionAndRotation(spawnLoadedAvatar.position, spawnLoadedAvatar.rotation);
        onAvatarCreatedEvent.Invoke();

        UpdateDebugText($"Successfully setup preview avatar");
    }

    public void CustomPropertiesDebug() {
        playerProperties["playerAvatarUrl"] = "https://d1a370nemizbjq.cloudfront.net/209a1bc2-efed-46c5-9dfd-edc8a1d9cbe4.glb";
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        //Debug.Log($"playerProperties: {(string)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatarUrl"]}");
    }

    private void Destroy() {
        displayButton.onClick.RemoveListener(DisplayWebView);
        closeButton.onClick.RemoveListener(HideWebView);
    }

    public void UpdateDebugText(string text) {
        debugText.text = text;
    }
}
