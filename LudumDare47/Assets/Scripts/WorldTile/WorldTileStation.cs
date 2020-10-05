using Enum;
using Manager;
using UnityEngine;

namespace WorldTile
{
    public class WorldTileStation : WorldTileSpecification
    {
        public int UpgradeLevel;

        public WorldTileStation(int level)
        {
            UpgradeLevel = level;
            Type = WorldTileSpecificationType.Station;

            if (SpriteManager.Instance.TryGetSpriteByName(spriteName: $"station_{UpgradeLevel}", outSprite: out Sprite sprite))
            {
                Sprite = sprite;
                SoundManager.Instance.PlaySoundPlaceStation();
            }
        }

        public override void OnDelete()
        {
            //Nix
        }
    }
}
