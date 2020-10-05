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

        /*[ReadOnly]*/ public WorldTileSpecificationType worldTileSpecificationType;

        public WorldTileSpecification WorldTileSpecification;

        private void Awake()
        {
            worldTileSpecificationType = WorldTileSpecificationType.None;
        }

        private void OnDestroy()
        {
            SoundManager.Instance.PlaySoundRemove();
        }

        public void InstantiateForShop(WorldTileSpecificationType worldTileSpecification, int level)
        {
            worldTileSpecificationType = worldTileSpecification;
            Sprite outSprite;

            switch (worldTileSpecification)
            {
                case WorldTileSpecificationType.None:
                    break;
                case WorldTileSpecificationType.Rail:
                    if (SpriteManager.Instance.TryGetSpriteByName(spriteName: $"rail_straight_{level}", outSprite: out outSprite))
                    {
                        sprite.sprite = outSprite;
                    }
                    break;
                case WorldTileSpecificationType.Station:
                    if (SpriteManager.Instance.TryGetSpriteByName(spriteName: $"station_{level}", outSprite: out outSprite))
                    {
                        sprite.sprite = outSprite;
                    }
                    break;
                case WorldTileSpecificationType.Environment:
                    string spriteName = UnityEngine.Random.Range(0,1) == 1 ? "tree" : "rock";
                    if (SpriteManager.Instance.TryGetSpriteByName(spriteName: spriteName, outSprite: out outSprite))
                    {
                        sprite.sprite = outSprite;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(worldTileSpecification), worldTileSpecification, null);
            }
        }

        public void Instantiate(int id, Vector2 pos, WorldTileSpecificationType worldTileSpecification, List<WorldTileClass> neighbours, int level)
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
                    WorldTileSpecification = new WorldTileRail(parent: this, neighbours: neighbours, level);

                    break;
                case WorldTileSpecificationType.Station:
                    WorldTileSpecification = new WorldTileStation(level);
                    break;
                case WorldTileSpecificationType.Environment:
                    WorldTileSpecification = new WorldTileEnvironment();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(paramName: nameof(worldTileSpecification),
                                                          actualValue: worldTileSpecification,
                                                          message: null);
            }

            sprite.sprite = WorldTileSpecification.Sprite;
            WorldTileSpecification.x = (int)this.position.x;
            WorldTileSpecification.y = (int)this.position.y;
        }

        public void UpdateSprite()
        {
            sprite.sprite = WorldTileSpecification?.Sprite;
        }

        public void OnDelete()
        {
            WorldTileSpecification.OnDelete();
            if (worldTileSpecificationType == WorldTileSpecificationType.Rail)
            {
                foreach (TrainMovment train in FindObjectsOfType<TrainMovment>())
                {
                    train.CheckIfTrackStillLoop();
                };
            }
            
        }
    }
}
