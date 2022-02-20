using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualSlideShow : MonoBehaviour
{
    [SerializeField] Image imageTarget;
    [SerializeField] List<Sprite> sprites;

    int currentSpriteIdx = 0;

    private void Start()
    {
        int currentSpriteIdx = 0;
        imageTarget.sprite = sprites[currentSpriteIdx];
    }

    public void NextSprite()
    {
        if (currentSpriteIdx + 1 == sprites.Count)
        {
            currentSpriteIdx = 0;
        }
        else
        {
            currentSpriteIdx += 1;
        }
        imageTarget.sprite = sprites[currentSpriteIdx];
    }

    public void PreviousSprite()
    {
        if (currentSpriteIdx - 1 < 0)
        {
            currentSpriteIdx = sprites.Count - 1;
        }
        else
        {
            currentSpriteIdx -= 1;
        }
        imageTarget.sprite = sprites[currentSpriteIdx];
    }
}
