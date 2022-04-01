using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using Photon.Realtime;

public class PhotonViewIsMineEvent : MonoBehaviourPunCallbacks
{
    enum INVOKE_TYPE { OnAwake, OnStart, OnEvent }

    [SerializeField] INVOKE_TYPE invokeType;
    [SerializeField] new PhotonView photonView;

    [Header("Unity Event"), Space(10)]
    [SerializeField] private List<GameObject> destroyOnNotMine;

    [Header("Unity Event"), Space(10)]
    [SerializeField] private UnityEvent onMineEvent;
    [SerializeField] private UnityEvent notMineEvent;

    private void Awake() {
        if (invokeType == INVOKE_TYPE.OnAwake) {
            InvokeEvent();
        }
    }

    private void Start() {
        if (invokeType == INVOKE_TYPE.OnStart) {
            InvokeEvent();
        }
    }

    public void InvokeEvent() {
        if (photonView.IsMine)
            onMineEvent.Invoke();
        else {
            foreach (GameObject item in destroyOnNotMine) {
                Destroy(item);
            }
            notMineEvent.Invoke();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        base.OnPlayerEnteredRoom(newPlayer);

        InvokeEvent();
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        InvokeEvent();
    }
}
