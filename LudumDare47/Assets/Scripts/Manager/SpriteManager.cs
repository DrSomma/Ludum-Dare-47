using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Manager
{
    public class SpriteManager : MonoBehaviour
    {
        #region SINGLETON PATTERN

        private static SpriteManager _instance;

        public static SpriteManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SpriteManager>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject(name: "SpriteManager");
                        _instance = container.AddComponent<SpriteManager>();
                    }
                }

                return _instance;
            }
        }

        #endregion

        public NamedSprite[] nameBySprite;
        private static Dictionary<string, Sprite> _nameBySprite;

        private void Awake()
        {
            _nameBySprite = nameBySprite.ToDictionary(keySelector: ns => ns.name, elementSelector: ns => ns.sprite);
        }

        public static bool TryGetSpriteByName(string spriteName, out Sprite outSprite)
        {
            return _nameBySprite.TryGetValue(key: spriteName, value: out outSprite);
        }
    }

    [Serializable]
    public struct NamedSprite
    {
        public string name;
        public Sprite sprite;
    }
}