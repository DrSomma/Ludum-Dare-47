using System.Runtime.CompilerServices;
using Enum;
using Manager;
using UnityEngine;

namespace WorldTile
{
    public class WorldTileRail : WorldTileSpecification
    {
        public WorldTileRail()
        {
            Type = WorldTileSpecificationType.Rail;
            if (SpriteManager.Instance.TryGetSpriteByName(spriteName: "rail_direction_E", outSprite: out Sprite sprite))
            {
                Sprite = sprite;
            }
        }
    }
}