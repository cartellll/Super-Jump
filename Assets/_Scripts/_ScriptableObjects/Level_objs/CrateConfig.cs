using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrateConfig", menuName = "Configs/Crate Config")]
public class CrateConfig : AbstractSpawnConfig
{
    [SerializeField] private GameObject deathEffectPrefab;
    public GameObject DeathEffectPrefab => deathEffectPrefab;
}