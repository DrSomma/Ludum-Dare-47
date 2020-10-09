using Enum;
using Manager;
using UnityEngine;

public class SpawnMap : MonoBehaviour
{
    public GameObject prefab;
    public int cntGras = 6;

    private void Start()
    {
        for (int x = 0; x < GameManager.Instance.width; x++)
        {
            for (int y = -1; y < GameManager.Instance.height + 1; y++)
            {
                GameObject temp = Instantiate(original: prefab, parent: transform, worldPositionStays: true);
                temp.transform.position = new Vector2(x: x, y: y);
                SpriteRenderer render = temp.GetComponentInChildren<SpriteRenderer>();

                string randomName = $"gras_{Random.Range(min: 0, max: cntGras)}";
                if (SpriteManager.TryGetSpriteByName(spriteName: randomName, outSprite: out Sprite sprite))
                {
                    render.sprite = sprite;
                    render.sortingOrder = -1;
                }

                if (Random.Range(min: 0f, max: 1f) > 0.8f)
                {
                    GameManager.Instance.BuildSomethingForced(x: x, y: y, buildType: WorldTileSpecificationType.Environment);
                }
            }
        }
    }
}