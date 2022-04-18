using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SuperStars.Events;
public class MoneyTextController : MonoBehaviour
{
    [SerializeField] private Text text;
    private void OnEnable()
    {
        EventManager.StartListening<int>(MoneyEvents.SetMoney, OnSetText);
    }

    private void OnDisable()
    {
        EventManager.StopListening<int>(MoneyEvents.SetMoney, OnSetText);
    }

    private void OnSetText(int money)
    {
        text.text = money.ToString();
    }
}
