using amazeIT;
using Enum;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldTile;

public class ShopManager : MonoBehaviour
{
    public GameObject cursor;
    public Dictionary<int,int> PriceTable;

    private SpriteRenderer _spriteRenderer;
    private WorldTileSpecificationType _buildType = WorldTileSpecificationType.Station;
    private int _buildPrice;
    private GameObject _tempBuilding;
    private bool onDestroyMode;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = cursor.GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = null;
        //_spriteRenderer.material.SetFloat("_GrayscaleAmount", 1);

        ShopItem.OnShopItemPressed += SetBuildType;

    }

    public void SetBuildType(ShopItem item)
    {
        if(item.Type == WorldTileSpecificationType.None)
        {
            SetDestroyMode();
        }
        else
        {
            SetBuildType(item.Type);
            _buildPrice = item.Price;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(button: 1))
        {
            GameManager.Instance.buildModeOn = false;
            _spriteRenderer.sprite = null;
            onDestroyMode = false;
            return;
        }

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position: Input.mousePosition);
        Utils.GetXY(worldPosition: worldPosition, x: out int x, y: out int y);
        cursor.transform.position = new Vector2(x, y);

        if (GameManager.Instance.buildModeOn)
        {
            if (GameManager.Instance.GetFieldStatus(x, y,out _).HasFlag(WorldTileStatusType.Buildable))
            {
                _spriteRenderer.color = Color.white;
                if (Input.GetMouseButtonDown(button: 0) && CanBuy(_buildPrice))
                {
                    GameManager.Instance.changeMoney(-_buildPrice);
                    GameManager.Instance.BuildSomething(x, y, _buildType);
                }
            }
            else
            {
                _spriteRenderer.color = Color.red;
            }
        }else if(onDestroyMode && Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.DeletTile(x, y);
        }
    }

    public void SetBuildType(WorldTileSpecificationType type)
    {
        _buildType = type;

        WorldTileClass worldTile = cursor.GetComponent<WorldTileClass>();

        worldTile.Instantiate(worldTileSpecification: type);

        GameManager.Instance.buildModeOn = true;

        _spriteRenderer.material.SetFloat("_GrayscaleAmount", 1);
    }

    public void SetDestroyMode()
    {
        onDestroyMode = true;
        GameManager.Instance.buildModeOn = false;
        if(SpriteManager.Instance.TryGetSpriteByName("bulldozer", out Sprite bulli)){
            _spriteRenderer.sprite = bulli;
        }
        _spriteRenderer.color = Color.white;
        _spriteRenderer.material.SetFloat("_GrayscaleAmount", 0);
    }

    private bool CanBuy(int price)
    {
        return GameManager.Instance.Money >= price;
    }

}
