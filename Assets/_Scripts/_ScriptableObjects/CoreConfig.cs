using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Core Config",menuName = "Configs/Core Config")]
public class CoreConfig : ScriptableObject
{
    [SerializeField] private PlayerConfig playerConfig;
    [SerializeField] private LevelController[] levels;

    public LevelController[] Levels => levels;
    public PlayerConfig PlayerConfig => playerConfig;

  
}
