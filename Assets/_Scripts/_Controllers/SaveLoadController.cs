using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperStars.Events;

public class SaveLoadController: MonoBehaviour
{
    GameSaveValue mGameSaveValue;
   
    bool ClassInitiate = false;

    private void OnEnable()
    {
        EventManager.StartListening<int>(MoneyEvents.AddMoney,AddMoney);
    }

    private void OnDisable()
    {
        EventManager.StopListening<int>(MoneyEvents.AddMoney, AddMoney);
    }
    public void RestartGame()
    {
        mGameSaveValue = new GameSaveValue();
        SaveParametrs();
    }

    public void InitiateClass()
    {
        if (ClassInitiate)
        {
            return;
        }

        if (PlayerPrefs.HasKey("SaveSuperJump"))
        {
            string SaveConquestElcastle = PlayerPrefs.GetString("SaveSuperJump");
            mGameSaveValue = JsonUtility.FromJson<GameSaveValue>(SaveConquestElcastle);
        }
        else
        {
            mGameSaveValue = new GameSaveValue();
        }

        ClassInitiate = true;
    }

    public void SetLevelNumber(int levelNumber)
    {
        InitiateClass();
        mGameSaveValue.levelNumber = levelNumber;
    }

    public int GetLevelNumber()
    {
        InitiateClass();
        return mGameSaveValue.levelNumber;
    }

    public void SaveParametrs()
    {
        InitiateClass();
        string scstring = JsonUtility.ToJson(mGameSaveValue);
        PlayerPrefs.SetString("SaveSuperJump", scstring);
    }

    public void AddMoney(int moneyValue)
    {
        InitiateClass();
        mGameSaveValue.moneyCount += moneyValue;
        SaveParametrs();
    }

    public int GetMoneyCount()
    {
        InitiateClass();
        return mGameSaveValue.moneyCount;
    }

    public void SetMoneyCount(int moneyValue)
    {
        InitiateClass();
        mGameSaveValue.moneyCount = moneyValue;
        SaveParametrs();
    }

    public void AddMaxHP(float maxHP)
    {
        InitiateClass();
        mGameSaveValue.maxPlayerHP += maxHP;
        SaveParametrs();
    }

    public float GetMaxHP()
    {
        InitiateClass();
        return mGameSaveValue.maxPlayerHP;
    }

    public void SetMaxHP(float maxHP)
    {
        InitiateClass();
        mGameSaveValue.maxPlayerHP = maxHP;
        SaveParametrs();
    }

    public void AddDamage(float damage)
    {
        InitiateClass();
        mGameSaveValue.maxPlayerHP += damage;
        SaveParametrs();
    }

    public float GetDamage()
    {
        InitiateClass();
        return mGameSaveValue.damage;
    }

    public void SetDamage(float damage)
    {
        InitiateClass();
        mGameSaveValue.damage = damage;
        SaveParametrs();
    }


    public void AddSpeedReloaded(float speedReloaded)
    {
        InitiateClass();
        mGameSaveValue.speedReload += speedReloaded;
        SaveParametrs();
    }

    public void SetSpeedReloaded(float speedReloaded)
    {
        InitiateClass();
        mGameSaveValue.speedReload = speedReloaded;
        SaveParametrs();
    }

    public float GetSpeedReloaded()
    {
        InitiateClass();
        return mGameSaveValue.speedReload;
    }

    public int GetActiveAttackLevelID()
    {
        InitiateClass();
        return mGameSaveValue.activeAttackLevelID;
    }

    public void SetActiveAttackLevelID(int attackLevelID)
    {
        InitiateClass();
        mGameSaveValue.activeAttackLevelID = attackLevelID;
        SaveParametrs();
    }
    public int GetActiveHPLevelID()
    {
        InitiateClass();
        return mGameSaveValue.activeHPLevelID;
    }

    public void SetActiveHPLevelID(int hpLevelID)
    {
        InitiateClass();
        mGameSaveValue.activeHPLevelID = hpLevelID;
        SaveParametrs();
    }
    
    public int GetActivePopupSkillsID()
    {
        InitiateClass();
        return mGameSaveValue.activePopupSkillsID;
    }

    public void SetActivePopupSkillsID(int levelId)
    {
        InitiateClass();
        mGameSaveValue.activePopupSkillsID = levelId;
        SaveParametrs();
    }


    [System.Serializable]
    public class GameSaveValue
    {
        public int levelNumber = 0;
        public int moneyCount = 0;

        public float maxPlayerHP = -1;
        public float damage = -1;
        public float speedReload = -1;

        public int activeAttackLevelID = 0;
        public int activeHPLevelID = 0;

        public int activePopupSkillsID = 0;
    }


}


