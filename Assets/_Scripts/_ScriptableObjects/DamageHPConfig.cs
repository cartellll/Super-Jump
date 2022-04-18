using UnityEngine;

[CreateAssetMenu(fileName = "DamageHPConfig", menuName = "Configs/DamageHP", order = 3)]
public class DamageHPConfig : ScriptableObject
{
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject deathEffect;

    [Min(0)]
    [SerializeField] private int maxHP;

    [Min(0)]
    [SerializeField] private int damage;
    public float MaxHP => maxHP;
    public float Damage => damage;
    public GameObject HitEffect => hitEffect;
    public GameObject DeathEffect => deathEffect;
}
