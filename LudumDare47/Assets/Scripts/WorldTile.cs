using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldTile : MonoBehaviour
{
    public SpriteRenderer sprite;
    public SpriteClass currentSelectedSpriteClass;

    private int currentSelectedSpriteIndex;

    public List<SpriteClass> sprites;

    private void Start()
    {
        UpdateSprite(index: 1);
    }

    private void UpdateSprite(int index)
    {
        Debug.Log(message: $"Change the sprite to {index}.");
        if (index >= 0 && index <= sprites.Count - 1)
        {
            currentSelectedSpriteIndex = index;
            currentSelectedSpriteClass = sprites[index: index];
            sprite.sprite = currentSelectedSpriteClass.sprite;
        }
    }

    public void ChangeSprite()
    {
        UpdateSprite(index: (currentSelectedSpriteIndex + 1) % sprites.Count);
    }
}