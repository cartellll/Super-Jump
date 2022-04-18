using UnityEngine;

[CreateAssetMenu(fileName = "BattleConfig", menuName = "Configs/BattleConfig", order = 3)]
public class BattleConfig : ScriptableObject
{
    [SerializeField] private GameObject attackEffect;
    public GameObject AttackEffect => attackEffect;
}