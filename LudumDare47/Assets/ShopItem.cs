using Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public int Price = 100;
    public WorldTileSpecificationType Type;

    public TextMeshProUGUI PriceTag; 

    public delegate void ShopItemPressed(ShopItem item);
    public static event ShopItemPressed OnShopItemPressed;


    // Start is called before the first frame update
    void Start()
    {
        if(PriceTag != null)
        {
            PriceTag.text = $"{Price}$";
        }
    }

    public void SetBuyItem()
    {
        OnShopItemPressed(this);
    }
}
