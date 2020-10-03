using UnityEngine;

[System.Serializable]
public class SpriteClass
{
    public string name;
    public Sprite sprite;

    public SpriteClass(string name, Sprite sprite)
    {
        this.name = name;
        this.sprite = sprite;
    }
}