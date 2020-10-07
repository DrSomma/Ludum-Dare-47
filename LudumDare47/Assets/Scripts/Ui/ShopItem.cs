using Enum;
using UnityEngine;
using TMPro;
using Manager;

public class ShopItem : MonoBehaviour
{
    public int price = 100;
    public int level = 0;
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
        UpdateUI(GameManager.Instance.startMoney, 0);

        GameManager.Instance.OnMoneyChanged += UpdateUI;
    }

    public void UpdateUI(int money,int sumToAdd)
    {
        if(money < price)
        {
            priceTag.color = Color.red;
        }
        else
        {
            priceTag.color = Color.white;
        }
    }

    public void SetBuyItem()
    {
        OnShopItemPressed?.Invoke(item: this);
    }
}