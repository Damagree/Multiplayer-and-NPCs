using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks {

    [SerializeField] private UnityEvent onJoinedRoom;

    private string destinationScene;

    public void CreateRoom(string name) {
        destinationScene = name;
        PhotonNetwork.CreateRoom(destinationScene);
    }

    public void JoinRoom(string name) {
        PhotonNetwork.JoinRoom(name);
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        onJoinedRoom.Invoke();
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogError($"Create Room Failed: [{returnCode}] {message}");
        JoinRoom(destinationScene);
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.LogError($"Join Room Failed: [{returnCode}] {message}");
    }
}
