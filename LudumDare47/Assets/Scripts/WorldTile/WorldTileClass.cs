using System;
using Enum;
using UnityEngine;
using Utils;

namespace WorldTile
{
    public class WorldTileClass : MonoBehaviour
    {
        public SpriteRenderer sprite;

        [ReadOnly] public WorldTileSpecificationType worldTileSpecificationType;

        private WorldTileSpecification _worldTileSpecification;

        public void Instantiate(WorldTileSpecificationType worldTileSpecification)
        {
            switch (worldTileSpecification)
            {
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