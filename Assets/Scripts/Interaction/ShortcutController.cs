using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class ShortcutController : MonoBehaviour
{
    [Serializable]
    struct Shortcut
    {
        public KeyCode keyCode;
        public UnityEvent eventTrigger;
    };

    [SerializeField] List<Shortcut> shortcuts;

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
