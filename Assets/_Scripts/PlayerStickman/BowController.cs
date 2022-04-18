using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : MonoBehaviour
{
    [SerializeField] private Weapon gun;
    public Weapon Gun => gun;
    private CrowdStickman enemyCrowd;

    public void InitWeapon(Weapon weapon)
    {
        gun = weapon;
    }
    private void Start()
    {
        enemyCrowd = GameManager.Instance.LevelManager.CurrentLevel.EnemyCrowd;
    }
    public void Attack()
    {
        if (enemyCrowd.GetCount() != 0)
        {
            gun.Attack(enemyCrowd.GetStickmanByIndex(Random.Range(0, enemyCrowd.GetCount())).gameObject, "sds", 5);
        }
    }
}
