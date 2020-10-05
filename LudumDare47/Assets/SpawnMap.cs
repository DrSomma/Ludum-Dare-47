using Enum;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMap : MonoBehaviour
{
    public GameObject prefab;
    public int cntGras = 6;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < GameManager.Instance.width; x++)
        {
            for (int y = -1; y < GameManager.Instance.height+1; y++)
            {
                GameObject temp =  Instantiate(prefab);
                temp.transform.position = new Vector2(x, y);
                SpriteRenderer render = temp.GetComponentInChildren<SpriteRenderer>();
                //tiles[Random.Range(0, tiles.Length-1)];
                string randomName = $"gras_{Random.Range(0, cntGras)}";
                if(SpriteManager.Instance.TryGetSpriteByName(randomName, out Sprite sprite))
                {
                    render.sprite = sprite;
                    render.sortingOrder = -1;
                }

                if(Random.Range(0f, 1f) > 0.8f)
                {
                    GameManager.Instance.BuildSomethingForced(x, y, WorldTileSpecificationType.Environment);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
