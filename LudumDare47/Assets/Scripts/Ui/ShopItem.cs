using Enum;
using Manager;
using TMPro;
using UnityEngine;

namespace Ui
{
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

            UpdateUI(money: GameManager.Instance.startMoney, sumToAdd: 0);

            GameManager.Instance.OnMoneyChanged += UpdateUI;
        }

        private void UpdateUI(int money, int sumToAdd)
        {
            priceTag.color = money < price ? Color.red : Color.white;
        }

        public void SetBuyItem()
        {
            OnShopItemPressed?.Invoke(item: this);
        }
    }
}