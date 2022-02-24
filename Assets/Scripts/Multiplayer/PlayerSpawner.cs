using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour {

    [SerializeField] GameObject[] playerPrefab;
    [SerializeField] Transform spawnPosition;

    private void Start() {
        GameObject playerObj = playerPrefab[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        GameObject playerToSpawn = PhotonNetwork.Instantiate(playerObj.name, spawnPosition.position, Quaternion.identity);
        playerToSpawn.transform.SetParent(spawnPosition.parent);
    }
}
