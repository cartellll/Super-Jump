using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SuperStars.Events;
public class PlayerBattleController : BattleController
{
    [SerializeField] protected float tapChangeTimeSpeed;
    [SerializeField] protected float decreaseTimeSpeed;

    [SerializeField] protected float maxReloadTime;
    [SerializeField] protected float minReloadTime;


    private void Start()
    {
        Init();
        enemyCrowd = GameManager.Instance.LevelManager.CurrentLevel.EnemyCrowd;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.StartListening<Vector3>(InputEvents.Input_Start, OnInputStart);
    }

    protected override void OnDisable()
    {
        base.OnEnable();
        EventManager.StopListening<Vector3>(InputEvents.Input_Start, OnInputStart);
    }

    private void OnInputStart(Vector3 position)
    {
        if (!isBattle)
        {
            return;
        }

        if (GameManager.Instance.state != GameManager.State.Play)
        {
            return;
        }

        ChangeSpeedReload(-tapChangeTimeSpeed);
    }


    private void Update()
    {
        UpdateBattle();
    }

    protected override void Init()
    {
        reloadTime= maxReloadTime;
        base.Init();
    }

    public void AppointEnemyStickman(DamageHPManager enemyStickman)
    {
        if(!IsBattle)
        {
            return;
        }

        if (targetEnemy != enemyStickman && isForceEnemy == false)
        {
            targetEnemy = enemyStickman;
            isForceEnemy = true;
        }
    }

    protected override void UpdateBattle()
    {
        base.UpdateBattle();
        ChangeSpeedReload(decreaseTimeSpeed * Time.deltaTime);

        animator.SetFloat("PunchSpeed", currentAnimationClip.length / reloadTime);
        /* if (animator.GetCurrentAnimatorStateInfo(0).IsName("Punching"))
         {
             animator.SetFloat("PunchSpeedTwo", currentAnimationClip.length / reloadTime);
         }
         else if(animator.GetCurrentAnimatorStateInfo(0).IsName("PunchingTwo"))
         {
             animator.SetFloat("PunchSpeed", currentAnimationClip.length / reloadTime);
         }*/        
    }

    private void ChangeSpeedReload(float difference)
    {
        reloadTime += difference;
        reloadTime = Mathf.Clamp(reloadTime, minReloadTime, maxReloadTime);
    }

}
