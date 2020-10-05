using System;
using System.Collections.Generic;
using amazeIT;
using Enum;
using Manager;
using UnityEngine;

namespace WorldTile
{
    public class WorldTileClass : MonoBehaviour
    {
        public SpriteRenderer sprite;
        public Vector2 position;
        public int objectId;

        [ReadOnly] public WorldTileSpecificationType worldTileSpecificationType;

        public WorldTileSpecification WorldTileSpecification;

        private void Awake()
        {
            worldTileSpecificationType = WorldTileSpecificationType.None;
        }

        public void InstantiateForShop(WorldTileSpecificationType worldTileSpecification)
        {
            worldTileSpecificationType = worldTileSpecification;
            Sprite outSprite;

            switch (worldTileSpecification)
            {
                case WorldTileSpecificationType.None:
                    break;
                case WorldTileSpecificationType.Rail:
                    if (SpriteManager.Instance.TryGetSpriteByName(spriteName: "rail_straight", outSprite: out outSprite))
                    {
                        sprite.sprite = outSprite;
                    }
                    break;
                case WorldTileSpecificationType.Station:
                    if (SpriteManager.Instance.TryGetSpriteByName(spriteName: "station", outSprite: out outSprite))
                    {
                        sprite.sprite = outSprite;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(worldTileSpecification), worldTileSpecification, null);
            }
        }

        public void Instantiate(int id, Vector2 pos, WorldTileSpecificationType worldTileSpecification, List<WorldTileClass> neighbours)
        {
            objectId = id;
            position = pos;
            worldTileSpecificationType = worldTileSpecification;

            switch (worldTileSpecification)
            {
                case WorldTileSpecificationType.None:
                    worldTileSpecificationType = WorldTileSpecificationType.None;
                    return;
                case WorldTileSpecificationType.Rail:
                    WorldTileSpecification = new WorldTileRail(parent: this, neighbours: neighbours);
                    break;
                case WorldTileSpecificationType.Station:
                    WorldTileSpecification = new WorldTileStation();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(paramName: nameof(worldTileSpecification),
                                                          actualValue: worldTileSpecification,
                                                          message: null);
            }

            sprite.sprite = WorldTileSpecification.Sprite;
        }

        public void UpdateSprite()
        {
            sprite.sprite = WorldTileSpecification?.Sprite;
        }
    }
}