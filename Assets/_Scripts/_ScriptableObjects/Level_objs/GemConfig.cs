using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GemConfig", menuName = "Configs/Gem Config")]
public class GemConfig : AbstractSpawnConfig
{
    [SerializeField] private GameObject deathEffectPrefab;
    public GameObject DeathEffectPrefab => deathEffectPrefab;

    [SerializeField] private float speedRotation;
    public float SpeedRotation => speedRotation;
}
