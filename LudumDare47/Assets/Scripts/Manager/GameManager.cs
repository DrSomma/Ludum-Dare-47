using amazeIT;
using System;
using System.Collections.Generic;
using Enum;
using UnityEngine;
using WorldTile;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }

        public GameObject worldTilePrefab;
        public int width = 18;
        public int height = 10;
        public bool drawDebugLine = true;

        [Header(header: "Building Settings")]
        public bool buildModeOn;

        public WorldTileSpecificationType
            currentSelectedWorldTileSpecificationType = WorldTileSpecificationType.Station;

        private Dictionary<KeyValuePair<int, int>, WorldTileClass> _gridByTile;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

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
            //if (Input.GetMouseButtonDown(button: 0))
            //{
            //    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position: Input.mousePosition);
            //    Utils.GetXY(worldPosition: worldPosition, x: out int x, y: out int y);

            //    if (buildModeOn)
            //    {
            //        BuildSomething(x: x, y: y);
            //    }
            //    else
            //    {
            //        GetInformation(x: x, y: y);
            //    }


            //    // DoActionOnWorldTile(x: x, y: y);
            //}
        }

        public void BuildSomething(int x, int y, WorldTileSpecificationType buildType)
        {
            WorldTileStatusType worldTileStatus = GetFieldStatus(x: x, y: y, worldTile: out WorldTileClass worldTile);

            if (worldTileStatus.HasFlag(flag: WorldTileStatusType.Invalid))
            {
                Debug.Log(message: "Invalid field - building impossible!");
                return;
            }
            else if (worldTileStatus.HasFlag(flag: WorldTileStatusType.Blocked))
            {
                Debug.Log(message: "Blocked field - building impossible!");
            }
            else if (worldTileStatus.HasFlag(flag: WorldTileStatusType.NotInitialized))
            {
                // Initialize
                GameObject gameObject = Instantiate(original: worldTilePrefab,
                                                    position: new Vector3(x: x, y: y),
                                                    rotation: Quaternion.identity);

                worldTile = gameObject.GetComponent<WorldTileClass>();

                worldTile.Instantiate(worldTileSpecification: buildType);

                _gridByTile.Add(key: new KeyValuePair<int, int>(key: x, value: y), value: worldTile);
            }
            else if (worldTileStatus.HasFlag(flag: WorldTileStatusType.Buildable))
            {
                worldTile.Instantiate(worldTileSpecification: buildType);
            }
        }

        public void DeletTile(int x, int y)
        {
            Debug.Log(_gridByTile.ContainsKey(key: new KeyValuePair<int, int>(key: x, value: y)));
            if (_gridByTile.TryGetValue(key: new KeyValuePair<int, int>(key: x, value: y),
                                             value: out WorldTileClass worldTile))
            {
                Destroy(worldTile.gameObject);
                _gridByTile.Remove(key: new KeyValuePair<int, int>(key: x, value: y));
            }
        }

        private void GetInformation(int x, int y)
        {
            WorldTileStatusType worldTileStatus = GetFieldStatus(x: x, y: y, worldTile: out WorldTileClass worldTile);

            if (worldTileStatus.HasFlag(flag: WorldTileStatusType.Invalid))
            {
                Debug.Log(message: "Invalid field!");
            }

            if (worldTileStatus.HasFlag(flag: WorldTileStatusType.Blocked))
            {
                Debug.Log(message: $"Blocked field: {worldTile.worldTileSpecificationType.ToString()}");
            }

            if (worldTileStatus.HasFlag(flag: WorldTileStatusType.NotInitialized))
            {
                Debug.Log(message: "Field not initialized!");
            }

            if (worldTileStatus.HasFlag(flag: WorldTileStatusType.Buildable))
            {
                Debug.Log(message: "Field is buildable!");
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

                    worldTile.Instantiate(worldTileSpecification: currentSelectedWorldTileSpecificationType);

                    _gridByTile.Add(key: new KeyValuePair<int, int>(key: x, value: y), value: worldTile);
                }
            }
        }

        private bool IsValidField(int x, int y)
        {
            return x >= 0 && y >= 0 && x < width && y < height;
        }

        public WorldTileStatusType GetFieldStatus(int x, int y, out WorldTileClass worldTile)
        {
            if (!IsValidField(x: x, y: y))
            {
                worldTile = null;
                return WorldTileStatusType.Invalid;
            }

            if (_gridByTile.TryGetValue(key: new KeyValuePair<int, int>(key: x, value: y),
                                        value: out worldTile))
            {
                switch (worldTile.worldTileSpecificationType)
                {
                    case WorldTileSpecificationType.None: return WorldTileStatusType.Buildable;
                    case WorldTileSpecificationType.Rail: return WorldTileStatusType.Blocked;
                    case WorldTileSpecificationType.Station: return WorldTileStatusType.Upgradeable;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                return WorldTileStatusType.NotInitialized | WorldTileStatusType.Buildable;
            }
        }
    }
}