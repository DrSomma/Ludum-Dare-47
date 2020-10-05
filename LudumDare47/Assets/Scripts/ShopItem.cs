using Enum;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public int price = 100;
    public WorldTileSpecificationType type;

    public TextMeshProUGUI priceTag;

    public delegate void ShopItemPressed(ShopItem item);

    public static event ShopItemPressed OnShopItemPressed;

    private void Start()
    {
        if (priceTag != null)
        {
            priceTag.text = $"{price}$";
        }
    }

    public void SetBuyItem()
    {
        OnShopItemPressed?.Invoke(item: this);
    }
}