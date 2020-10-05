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

        public WorldTileSpecification WorldTileSpecification;

        private void Awake()
        {
            worldTileSpecificationType = WorldTileSpecificationType.None;
        }

        public void Instantiate(WorldTileSpecificationType worldTileSpecification)
        {
            //Get Pos
            Utils.GetXY(transform.position, out int x, out int y);

            switch (worldTileSpecification)
            {
                case WorldTileSpecificationType.None:
                    worldTileSpecificationType = WorldTileSpecificationType.None;
                    return;
                case WorldTileSpecificationType.Rail:
                    WorldTileSpecification = new WorldTileRail(x,y);
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
            worldTileSpecificationType = WorldTileSpecification.Type;
        }

        private void OnDrawGizmos()
        {
            Vector3 center = new Vector3(transform.position.x, transform.position.y, 0);
            Gizmos.DrawSphere(center, 0.1f);
            Gizmos.color = Color.red;
            Vector3 center2 = new Vector3(transform.position.x+0.5f, transform.position.y + 0.5f, 0);
            Gizmos.DrawSphere(center2, 0.1f);
        }
    }
}