using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour {

    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform spawnPosition;

    private void Start() {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, Quaternion.identity);
        playerObj.transform.SetParent(spawnPosition.parent);
    }
}
