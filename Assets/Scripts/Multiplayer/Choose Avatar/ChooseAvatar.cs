using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class ChooseAvatar : MonoBehaviour
{
    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    private void Start() {
        playerProperties["playerAvatar"] = 0;
    }

    public void SelectAvatar(int idx = 0) {
        playerProperties["playerAvatar"] = idx;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
}
