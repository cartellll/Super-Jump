using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelController CurrentLevel { get; private set; }
    public int CurrentLevelIndex { get; set; }

    public void LoadLevel(LevelController level,int levelIndex)
    {
        CurrentLevel = Instantiate(level);
        CurrentLevelIndex = levelIndex;
    }
}
