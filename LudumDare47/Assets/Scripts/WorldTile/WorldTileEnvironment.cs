using Enum;
using Manager;
using UnityEngine;

namespace WorldTile
{
    public class WorldTileEnvironment : WorldTileSpecification
    {
        public WorldTileEnvironment()
        {
            Type = WorldTileSpecificationType.Environment;

            string spriteName = UnityEngine.Random.Range(0f, 1f) > 0.5f ? "tree" : "stone";
            if (SpriteManager.TryGetSpriteByName(spriteName: spriteName, outSprite: out Sprite sprite))
            {
                Sprite = sprite;
            }
        }

        public override void OnDelete()
        {
            
        }
    }
}
