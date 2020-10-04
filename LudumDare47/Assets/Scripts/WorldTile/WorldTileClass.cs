using System;
using amazeIT;
using Enum;
using UnityEngine;

namespace WorldTile
{
    public class WorldTileClass : MonoBehaviour
    {
        public SpriteRenderer sprite;

        [ReadOnly] public WorldTileSpecificationType worldTileSpecificationType;

        private WorldTileSpecification _worldTileSpecification;

        private void Awake()
        {
            worldTileSpecificationType = WorldTileSpecificationType.None;
        }

        public void Instantiate(WorldTileSpecificationType worldTileSpecification)
        {
            switch (worldTileSpecification)
            {
                case WorldTileSpecificationType.None:
                    worldTileSpecificationType = WorldTileSpecificationType.None;
                    return;
                case WorldTileSpecificationType.Rail:
                    _worldTileSpecification = new WorldTileRail();
                    break;
                case WorldTileSpecificationType.Station:
                    _worldTileSpecification = new WorldTileStation();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(paramName: nameof(worldTileSpecification),
                                                          actualValue: worldTileSpecification,
                                                          message: null);
            }

            sprite.sprite = _worldTileSpecification.Sprite;
            worldTileSpecificationType = _worldTileSpecification.Type;
        }
    }
}