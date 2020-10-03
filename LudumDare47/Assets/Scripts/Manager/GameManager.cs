using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public GameObject worldTilePrefab;
        public int width = 18;
        public int height = 10;
        public bool DrawDebugLine = true;

        // private Grid _grid;
        private Dictionary<KeyValuePair<int, int>, WorldTile> _gridByTile;

        private void Start()
        {
            // _grid = new Grid(width: width, height: height);
            _gridByTile = new Dictionary<KeyValuePair<int, int>, WorldTile>();


            // Grid to make visible the border of playing field
            if (DrawDebugLine)
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
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                if (!_gridByTile.TryGetValue(key: new KeyValuePair<int, int>(key: x, value: y),
                                             value: out WorldTile worldTile))
                {
                    GameObject gameObject = Instantiate(original: worldTilePrefab,
                                                        position: new Vector3(x: x, y: y),
                                                        rotation: Quaternion.identity);

                    worldTile = gameObject.GetComponent<WorldTile>();

                    _gridByTile.Add(key: new KeyValuePair<int, int>(key: x, value: y), value: worldTile);
                }

                worldTile.ChangeSprite();
            }
        }

        private void ChangeColorOfTile(WorldTile worldTile)
        {
            if (worldTile.sprite != null)
            {
                worldTile.sprite.color = new Color(r: Random.value, g: Random.value, b: Random.value);
            }
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(f: worldPosition.x);
            y = Mathf.FloorToInt(f: worldPosition.y);
        }
    }
}