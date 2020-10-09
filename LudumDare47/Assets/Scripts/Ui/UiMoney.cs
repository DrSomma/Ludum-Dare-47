using Manager;
using TMPro;
using UnityEngine;

namespace Ui
{
    public class UiMoney : MonoBehaviour
    {
        private TextMeshProUGUI _textField;

        private void Start()
        {
            _textField = GetComponent<TextMeshProUGUI>();
            UpdateUI(money: GameManager.Instance.startMoney, sumToAdd: 0);
            GameManager.Instance.OnMoneyChanged += UpdateUI;
        }

        private void UpdateUI(int money, int sumToAdd)
        {
            _textField.text = $"{money}$";
        }
    }
}