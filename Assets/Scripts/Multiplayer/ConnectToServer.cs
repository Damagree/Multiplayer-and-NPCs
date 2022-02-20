using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class ConnectToServer : MonoBehaviourPunCallbacks {

    [SerializeField] private UnityEvent onJoinedLobby;

    // Start is called before the first frame update
    void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {
        base.OnJoinedLobby();
        onJoinedLobby.Invoke();
    }
}
