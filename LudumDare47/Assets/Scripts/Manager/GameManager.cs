using System.Collections.Generic;
using Enum;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using WorldTile;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public GameObject worldTilePrefab;
        public int width = 18;
        public int height = 10;
        public bool drawDebugLine = true;
        public WorldTileSpecificationType debugWorldTileSpecificationType = WorldTileSpecificationType.Station;

        private Dictionary<KeyValuePair<int, int>, WorldTileClass> _gridByTile;

        private void Start()
        {
            _gridByTile = new Dictionary<KeyValuePair<int, int>, WorldTileClass>();


            // Grid to make visible the border of playing field
            if (drawDebugLine)
            {
                Debug.DrawLine(start: new Vector3(x: 0, y: 0),
                               end: new Vector3(x: 0, y: height),
                               color: Color.white,
                               duration: 100f);
                Debug.DrawLine(start: new Vector3(x: 0, y: 0),
                               end: new Vector3(x: width, y: 0),
                               color: Color.white,
                               duration: 100f);
                Debug.DrawLine(start: new Vector3(x: 0, y: height),
                               end: new Vector3(x: width, y: height),
                               color: Color.white,
                               duration: 100f);
                Debug.DrawLine(start: new Vector3(x: width, y: 0),
                               end: new Vector3(x: width, y: height),
                               color: Color.white,
                               duration: 100f);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(button: 0))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position: Input.mousePosition);
                GetXY(worldPosition: worldPosition, x: out int x, y: out int y);

                DoActionOnWorldTile(x: x, y: y);
            }
        }

        private void DoActionOnWorldTile(int x, int y)
        {
            if (IsValidField(x: x, y: y))
            {
                if (!_gridByTile.TryGetValue(key: new KeyValuePair<int, int>(key: x, value: y),
                                             value: out WorldTileClass worldTile))
                {
                    GameObject gameObject = Instantiate(original: worldTilePrefab,
                                                        position: new Vector3(x: x, y: y),
                                                        rotation: Quaternion.identity);

                    worldTile = gameObject.GetComponent<WorldTileClass>();

                    worldTile.Instantiate(worldTileSpecification: debugWorldTileSpecificationType);

                    _gridByTile.Add(key: new KeyValuePair<int, int>(key: x, value: y), value: worldTile);
                }
            }
        }

        private bool IsValidField(int x, int y)
        {
            return x >= 0 && y >= 0 && x < width && y < height;
        }

        private static void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(f: worldPosition.x);
            y = Mathf.FloorToInt(f: worldPosition.y);
        }
    }
}