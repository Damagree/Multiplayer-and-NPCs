using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Photon.Pun;

public class ShortcutController : MonoBehaviour
{
    [Serializable]
    struct Shortcut
    {
        public KeyCode keyCode;
        public UnityEvent eventTrigger;
    };

    [Header("If using Photon")]
    [SerializeField] bool isUsingPhoton = true;
    [SerializeField] PhotonView photonView;

    [SerializeField] List<Shortcut> shortcuts;

    private void Start() {
        if (!isUsingPhoton) {
            return;
        }
        if (!photonView.IsMine) {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)   // Check if there's any key active
        {
            for (int i = 0; i < shortcuts.Count; i++) // loop every shortcuts
            {
                if (Input.GetKeyDown(shortcuts[i].keyCode)) // check if current key is same with te shortcuts
                {
                    shortcuts[i].eventTrigger.Invoke();     // invoke the event
                }
            }
        }
    }

}
