using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class GAManager : MonoBehaviour
{
    public static GAManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }
    void Start()
    {
        GameAnalytics.Initialize();
    }

    public void OnFirstStartGame()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start,"FirstStart");
    }

    public void OnStartGame()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Start");
    }

    public void OnFinishLevel(int id)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level" + id);
    }

    public void OnFailedLevel(int id)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level" + id);
    }

    public void OnPurchaseUpgrade(string itemName, float count , int levelID)
    {
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "Money", count, itemName,"Level:"+levelID);
    }

    public void ActivateTotem(int levelID, string totemName)
    {
        GameAnalytics.NewDesignEvent("Totem Active:Level" + levelID + ":" + totemName);
    }
}
