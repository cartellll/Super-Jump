using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stickman Config", menuName = "Configs/Stickman Config")]
public class PlayerStickmanConfig : AbstractSpawnConfig
{  
    [Min(0)]
    [SerializeField] private int size;
    [SerializeField] private float scale;
    [SerializeField] private Material material;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float speed;
    [SerializeField] private float hp;
    [SerializeField] private float damage;
    [SerializeField] private GameObject playerStickmanMergeEffect;

    public int Size => size;
    public float Scale => scale;
    public Material Material => material;
    public Material LineMaterial => lineMaterial;
    public float Speed => speed;
    public float HP => hp;
    public float Damage => damage;
    public GameObject PlayerStickmansMergeEffect => playerStickmanMergeEffect;
}
