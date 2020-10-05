using Enum;
using Manager;
using UnityEngine;

namespace WorldTile
{
    public class WorldTileStation : WorldTileSpecification
    {
        public WorldTileStation()
        {
            Type = WorldTileSpecificationType.Station;

            if (SpriteManager.Instance.TryGetSpriteByName(spriteName: "station", outSprite: out Sprite sprite))
            {
                Sprite = sprite;
            }
        }

        public override void OnDelete()
        {
            //Nix
        }
    }
}
