using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleController : BattleController
{
    private void Start()
    {
        Init();
        enemyCrowd = GameManager.Instance.LevelManager.CurrentLevel.PlayerCrowd;
    }
    private void Update()
    {
    
        UpdateBattle();
    }

    protected override void AttackEnemy()
    {
        base.AttackEnemy();

        if (targetEnemy != null)
        {
            targetEnemy.GetComponent<PlayerBattleController>()?.AppointEnemyStickman(damageHpManager);
        }
    }
}
