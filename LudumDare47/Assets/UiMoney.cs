using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Manager;

public class UiMoney : MonoBehaviour
{
    private TextMeshProUGUI _textField;

    // Start is called before the first frame update
    void Start()
    {
        _textField = GetComponent<TextMeshProUGUI>();
        GameManager.Instance.OnMoneyChanged += UpdateUI;
    }

    public void UpdateUI(int Money, int sumToAdd)
    {
        _textField.text = $"{Money}$";
    }



}
