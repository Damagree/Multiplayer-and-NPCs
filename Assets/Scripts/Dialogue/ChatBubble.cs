using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatBubble : MonoBehaviour {

    [SerializeField] Image background;
    [SerializeField] TextMeshProUGUI contentText;

    private void Start() {
        Setup("Because I've Been Reincarnated as the Piggy Duke, This Time I Will Say I Like You - Chapter 1");
    }

    private void Setup(string text) {
        contentText.SetText(text);
        contentText.ForceMeshUpdate();
        //Vector2 textSize = contentText.GetRenderedValues(false);
        //Vector2 padding = new Vector2(7f, 4f);
        //background.rectTransform.rect.Set(background.rectTransform.rect.x, background.rectTransform.rect.y, textSize.x + padding.x, textSize.y + padding.y);
    }

    private void OnEnable() {
        Setup("Because I've Been Reincarnated as the Piggy Duke, This Time I Will Say I Like You - Chapter 1");
    }

    private void OnDisable() {
        ResetText();
    }

    private void ResetText() {
        contentText.SetText("");
        /// TODO: reset value of dialogue path(?) to zero 
    }
}
