using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour {

    [SerializeField] bool singleCharacterType = true;
    [SerializeField] GameObject singlePlayerPrefab;
    [SerializeField] GameObject[] MultiPlayerPrefabs;
    [SerializeField] Transform spawnPosition;

    private void Start() {
        GameObject playerObj, playerToSpawn;

        if (singleCharacterType) {
            playerToSpawn = PhotonNetwork.Instantiate(singlePlayerPrefab.name, spawnPosition.position, Quaternion.identity);
        } else {
            playerObj = MultiPlayerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
            playerToSpawn = PhotonNetwork.Instantiate(playerObj.name, spawnPosition.position, Quaternion.identity);
        }

        playerToSpawn.transform.SetParent(spawnPosition.parent);
    }
}
