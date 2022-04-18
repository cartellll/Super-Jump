using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Config", menuName = "Enemy Config")]
public class EnemyConfig : AbstractSpawnConfig
{
    [SerializeField] private Vector3 scale;
    public Vector3 Scale => scale;

    [SerializeField] private float startSpeed;
    public float StartSpeed => startSpeed;

    [SerializeField] private Material whiteMaterial;
    public Material WhiteMaterial => whiteMaterial;
}
