using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BarrelConfig", menuName = "Configs/Barrel Config")]
public class BarrelConfig : AbstractSpawnConfig
{
    [SerializeField] private GameObject deathEffectPrefab;
    public GameObject DeathEffectPrefab => deathEffectPrefab;

    [SerializeField] private float speedMove;
    public float SpeedMove => speedMove;

    [SerializeField] private float speedRotation;
    public float SpeedRotation => speedRotation;
}
