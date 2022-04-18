using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SuperStars.Events;

public class Enemy : MonoBehaviour
{

    [SerializeField] private EnemyConfig config;
    public EnemyConfig Config
    {
        get
        {
            return config;
        }
        set
        {
            config = value;
            transform.localScale = config.Scale;
        }
    }
    [SerializeField] private Animator animator;
    [SerializeField] private SimpleRagdollHelper ragdollHelper;
    [SerializeField] private EnemyBattleController battleController;
    [SerializeField] private DamageHPManager damageHpManager;
    public DamageHPManager DamageHpManager => damageHpManager;

    [SerializeField] private StickmanUIController uiController;

   private float attackPositionZ;

    private enum State
    {
        Run,
        Battle
    }
    private State state = State.Run;

    private void Start()
    {
        ragdollHelper.Init();
        ragdollHelper.ragdolled = false;
        attackPositionZ = GameManager.Instance.LevelManager.CurrentLevel.GameField.EnemyAttackTrigger.transform.position.z;
    }

    private void OnEnable()
    {
        EventManager.StartListening<DamageHPManager>(DeathEvents.Death_stickman, OnDead);
    }

    private void OnDisable()
    {
        EventManager.StopListening<DamageHPManager>(DeathEvents.Death_stickman, OnDead);
    }
    
   

    void Update()
    {

        if (GameManager.Instance.state == GameManager.State.Finish && !animator.GetCurrentAnimatorStateInfo(0).IsName("Dance"))
        {
            battleController.IsBattle = false;
            transform.LookAt(Vector3.back);
            animator.SetBool("Dance", true);
            uiController.Text.gameObject.SetActive(false);
        }
        else
        {
            animator.SetBool("Dance", false);
        }

        if (GameManager.Instance.state != GameManager.State.Play)
        {
            animator.SetBool("Run", false);
            return;
        }

        switch (state)
        {
            case State.Run:
                Run();
                break;

            case State.Battle:
                break;
        }
    }

    private void Run()
    {
        // agent.SetDestination(Vector3.zero);

        animator.SetBool("Run", true);
        transform.position += Vector3.back * config.StartSpeed * Time.deltaTime;

        if(transform.position.z < attackPositionZ || GameManager.Instance.LevelManager.CurrentLevel.GetLevelState == LevelController.LevelState.Battle)
        {
            if (battleController != null)
            {
                GameManager.Instance.LevelManager.CurrentLevel.StartBattle();
                battleController.IsBattle = true;
                state = State.Battle;
            }
        }
    }

    private void OnDead(DamageHPManager tempDamageHpManager)
    {
        if (damageHpManager == tempDamageHpManager)
        {
            foreach (Renderer renderer in GetComponentsInChildren<MeshRenderer>())
            {
                renderer.material = config.WhiteMaterial;
            }
            foreach (Renderer renderer in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                renderer.material = config.WhiteMaterial;
            }

            StartCoroutine(Dead());
        }
    }

    public IEnumerator Dead()
    {
        damageHpManager.ActivateDeathEffect();
        battleController.IsBattle = false;
        ragdollHelper.ragdolled = true;
        yield return new WaitForSeconds(2);
      //  Destroy(gameObject);
    }
}
