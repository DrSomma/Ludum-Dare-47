using Enum;
using UnityEngine;

namespace WorldTile
{
    public abstract class WorldTileSpecification
    {
        public WorldTileSpecificationType Type;

        public Sprite Sprite;

        public int x;
        public int y;

        public abstract void OnDelete();
    }
}
