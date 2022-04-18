using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SuperStars.Events;

[RequireComponent(typeof(DamageHPManager))]
public abstract class BattleController : MonoBehaviour
{
    public enum PunchingState
    {
        Fisting,
        Sword,
        Archer
    }
    [SerializeField] protected NavMeshObstacle navMeshObstacle;
    [SerializeField] protected GameObject sizeCube;
    public GameObject SizeCube => sizeCube;
    [SerializeField] protected AnimationClip swordAimationClip;
    [SerializeField] protected AnimationClip fistAimationClip;
    [SerializeField] protected AnimationClip archerAttackAimationClip;
    [SerializeField] protected AnimationClip archerIdleAimationClip;
    protected AnimationClip currentAnimationClip;

    [SerializeField] protected AnimatorOverrideController animatorOverrideController;

    protected NavMeshAgent agent;
    protected Animator animator;
    protected DamageHPManager damageHpManager;
    protected DamageHPManager targetEnemy;
    protected CrowdStickman enemyCrowd;

    protected bool isBattle = false;

    [Min(0.01f)]
    [SerializeField] protected float reloadTime;

    protected float timerReloadTime;

    protected bool isForceEnemy = false;
    public bool IsBattle
    {
        get
        {
            return isBattle;
        }

        set
        {
            isBattle = value;
            if (isBattle == true)
            {
                agent.enabled = true;
            }

            else
            {
                agent.enabled = false;
                targetEnemy = null;
            }
        }
    }

    protected virtual void OnEnable()
    {
        EventManager.StartListening<DamageHPManager>(DeathEvents.Death_stickman, OnDead);
    }

    protected virtual void OnDisable()
    {
        EventManager.StopListening<DamageHPManager>(DeathEvents.Death_stickman, OnDead);
    }

    protected virtual void Init()
    {
        navMeshObstacle.enabled = false;
        currentAnimationClip = fistAimationClip;
        animator = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        timerReloadTime = 0;
        agent = GetComponent<NavMeshAgent>();
        damageHpManager = GetComponent<DamageHPManager>();
        IsBattle = false;

        animator.SetFloat("PunchSpeed", currentAnimationClip.length / reloadTime);
    }


    protected virtual void UpdateBattle()
    {
        //Debug.Log(gameObject.name + " " + sizeCube.transform.localScale.z * transform.localScale.z);
        if (!isBattle)
        {
            return;
        }

        if (GameManager.Instance.state != GameManager.State.Play)
        {
            return;
        }

        if (targetEnemy != null && targetEnemy.HP > 0)
        {
            Vector3 vectorToTarget = targetEnemy.transform.position - gameObject.transform.position;

            float distanceToTarget = new Vector2(sizeCube.transform.localScale.x / 2f, sizeCube.transform.localScale.z / 2f).magnitude;
            Transform enemySizeCube = targetEnemy.gameObject.GetComponent<BattleController>().sizeCube.transform;
            float enemyDistanceToTarget = new Vector2(enemySizeCube.localScale.x / 2f, enemySizeCube.localScale.z / 2f).magnitude;

            if (new Vector2(vectorToTarget.x,vectorToTarget.z).magnitude > 0.05f + distanceToTarget * transform.localScale.z + enemyDistanceToTarget * targetEnemy.transform.localScale.z)
            //if (new Vector2(vectorToTarget.x, vectorToTarget.z).magnitude > 0.1 + transform.localScale.z/3.5f + targetEnemy.transform.localScale.z/3.5f)
            {
                MoveToEnemy();
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(vectorToTarget.normalized);
                timerReloadTime -= Time.deltaTime;
                if (timerReloadTime <= 0)
                {
                    AttackEnemy();
                    timerReloadTime = reloadTime;
                }
            }
        }
        else
        {
            isForceEnemy = false;
            ChangeTargetEnemy();
        }
    }

    protected virtual void MoveToEnemy()
    {
        animator.SetBool("Run", true);
        animator.SetBool("Punching", false);

        navMeshObstacle.enabled = false;
        agent.enabled = true;

        if (agent.isOnNavMesh)
        {
            agent.SetDestination(targetEnemy.transform.position);
        }
    }

    protected virtual void AttackEnemy()
    {
        agent.enabled = false;
        navMeshObstacle.enabled = true;

        if (agent.isOnNavMesh)
        {
            agent.SetDestination(transform.position);
        }
        animator.SetBool("Run", false);
        animator.SetBool("Punching", true);

        targetEnemy.HP -= damageHpManager.Damage;
        targetEnemy.ActivateHitEffect();

    }
    protected virtual void ChangeTargetEnemy()
    {
        if (enemyCrowd.GetCount() != 0)
        {
            targetEnemy = enemyCrowd.GetStickmanByIndex(0);
            Vector3 vectorToTarget = targetEnemy.transform.position - gameObject.transform.position;

            //Находим ближайшего врага
            for (int i = 0; i < enemyCrowd.GetCount()-1; i++)
            {
                Vector3 vectorToNextTarget = enemyCrowd.GetStickmanByIndex(i+1).transform.position - gameObject.transform.position;
                if (new Vector2(vectorToTarget.x, vectorToTarget.z).magnitude > new Vector2(vectorToNextTarget.x, vectorToNextTarget.z).magnitude)
                {
                    targetEnemy = enemyCrowd.GetStickmanByIndex(i + 1);
                    vectorToTarget = vectorToNextTarget;
                }
            }
            
            animator.SetBool("Punching", false);
            animator.SetBool("Run", true);
        }
        else
        {
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(transform.position);
            }
            animator.SetBool("Punching", false);
            animator.SetBool("Run", false);
        }
    }

 
    public void ChangeAnimationAttack(PunchingState state)
    {
        switch (state)
        {
            case PunchingState.Fisting:
                currentAnimationClip = fistAimationClip;
                break;
            case PunchingState.Sword:
                currentAnimationClip = swordAimationClip;
                break;
            case PunchingState.Archer:
                animatorOverrideController["Idle"] = archerIdleAimationClip;
                currentAnimationClip = archerAttackAimationClip;
                break;

        }
        animatorOverrideController["Punching"] = currentAnimationClip;
        animator.SetFloat("PunchSpeed", currentAnimationClip.length / reloadTime);
    }

    private void OnDead(DamageHPManager tempDamageHpManager)
    {
        if (damageHpManager == tempDamageHpManager)
        {
            agent.enabled = false;
            navMeshObstacle.enabled = false;
        }
    }

}