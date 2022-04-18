using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Config",menuName = "Configs/Player Config")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private PlayerStickmanController playerStickmanPrefab;

    [SerializeField] private StickmanConfigs stickmanConfigs;

    [Min(0)]
    [SerializeField] private float timeToRestartStickman;

    public PlayerStickmanController PlayerStickmanPrefab => playerStickmanPrefab;
    public StickmanConfigs StickmanConfigs => stickmanConfigs;

    public float TimeToRestartStickman => timeToRestartStickman;
}
