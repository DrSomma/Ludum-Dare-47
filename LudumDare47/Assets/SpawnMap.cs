using Enum;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMap : MonoBehaviour
{
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < GameManager.Instance.width; x++)
        {
            for (int y = 0; y < GameManager.Instance.height; y++)
            {
                GameObject temp =  Instantiate(prefab);
                temp.transform.position = new Vector2(x, y);
                SpriteRenderer render = temp.GetComponentInChildren<SpriteRenderer>();
                //tiles[Random.Range(0, tiles.Length-1)];
                string randomName = $"gras_{Random.Range(0, 6)}";
                if(SpriteManager.Instance.TryGetSpriteByName(randomName, out Sprite sprite))
                {
                    render.sprite = sprite;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
