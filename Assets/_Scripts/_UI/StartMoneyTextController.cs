using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SuperStars.Events;
public class StartMoneyTextController : MonoBehaviour
{
    [SerializeField] private Text text;
    private void OnEnable()
    {
        EventManager.StartListening<int>(MoneyEvents.SetSaveMoney, OnSetText);
    }

    private void OnDisable()
    {
        EventManager.StopListening<int>(MoneyEvents.SetSaveMoney, OnSetText);
    }

    private void OnSetText(int money)
    {
        text.text = money.ToString();
    }
}
