using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperStars.Events;
using UnityEngine.AI;


[RequireComponent(typeof(DamageHPManager))]
public class PlayerStickmanController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStickmanConfig config;
    [SerializeField] private GeneralPlayerStickmanConfig generalConfig;

    public PlayerStickmanConfig Config
    {
        get
        {
            return config;
        }
        set
        {
            config = value;
        }
    }

    private DamageHPManager damageHpManager;
    public DamageHPManager DamageHpManager => damageHpManager;

    public int Size => growingUpController.Size;
   
    private Rigidbody rigidBody;

    [SerializeField] private Rigidbody spineRigidBody;

    private Collider[] colliders;

    [SerializeField] private GameObject ragdollStickman;


    [SerializeField] private RagdollHelper ragdollHelper;

    [Header("GrowingUpController")]
    [SerializeField] private GrowingUpController growingUpController;

    [Header("BattleController")]
    [SerializeField] private PlayerBattleController battleController;
    public PlayerBattleController BattleController => battleController;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Armor")]
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject shelmet;
    [SerializeField] private GameObject sword;

    [Header("UI")]
    [SerializeField] private StickmanUIController uiController;
    public GameObject Shield => shield;
    public GameObject Shelmet => shelmet;
    public GameObject Sword => sword;

    private bool isControlled = false;

    private LevelController levelController;


    private BowController bowController;

    public float Speed { get; set; }
    
    private bool _isInit = false;
    private void Awake()
    {
        damageHpManager = GetComponent<DamageHPManager>();
        colliders = ragdollStickman.GetComponentsInChildren<Collider>();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if(_isInit)
        {
            return;
        }
        SetEnabled(true);
        growingUpController.Init();
        levelController = GameManager.Instance.LevelManager.CurrentLevel;
        if (sword.activeSelf)
        {
            battleController.ChangeAnimationAttack(global::BattleController.PunchingState.Sword);
        }

        rigidBody = spineRigidBody;
        //rigidBody = GetComponent<Rigidbody>();

       // SetEnabled(false);
       // GetComponent<Collider>().enabled = true;
        _isInit = true;
    }

    public void InitBow(Weapon weapon)
    {
        bowController = gameObject.AddComponent<BowController>();
        bowController.InitWeapon(weapon);
        battleController.ChangeAnimationAttack(global::BattleController.PunchingState.Archer);
    }

    private void OnEnable()
    {
        EventManager.StartListening<DamageHPManager>(DeathEvents.Death_stickman,OnDead);
    }

    private void OnDisable()
    {
        EventManager.StopListening<DamageHPManager>(DeathEvents.Death_stickman, OnDead);
    }

    private void Update()
    {

        if(GameManager.Instance.state == GameManager.State.Finish && !animator.GetCurrentAnimatorStateInfo(0).IsName("Dance"))
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
            return;
        }
  
        if (levelController.GetLevelState == LevelController.LevelState.Battle)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Stand_up_from_back") || animator.GetCurrentAnimatorStateInfo(0).IsName("Stand_up_from_belly") || ragdollHelper.ragdolled)
            {
                battleController.IsBattle = false;
            }
            else if (battleController != null && !battleController.IsBattle && (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run")))
            {
                RemoveRigidBodyOnParent();
                battleController.IsBattle = true;
            }
        }
        uiController.UpdateVisibleText(ragdollHelper.ragdolled, damageHpManager.HP);
    }

    private Vector3 velocityBeforePhysicsUpdate;
    void FixedUpdate()
    {
        velocityBeforePhysicsUpdate = rigidBody.velocity;
    }

    public void UpdateParametrs()
    {
        growingUpController.Init();
        growingUpController.UpdateParametrs();
    }
    public void UpdateUI()
    {
        if (uiController != null)
        {
            uiController.UpdateText(damageHpManager.HP);
            uiController.UpdateTextSize(transform.localScale.x);
        }
    }

    public void SetEnabled(bool isEnabled)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = isEnabled;
        }
    }

    public void StartControlledSitckman()
    {
        if (bowController != null)
        {
            animator.SetBool("Punching", true);
            bowController.Attack();
            bowController.Gun.gameObject.SetActive(false);
            StartCoroutine(WaitStartControlledSitckman(0.5f));
        }
        else
        {
            StartCoroutine(WaitStartControlledSitckman(0));
        }
      
    }

    private IEnumerator WaitStartControlledSitckman(float time)
    {
        yield return new WaitForSeconds(time);
        rigidBody.isKinematic = false;
        rigidBody.AddForce(Vector3.forward * Speed);
        SetRunAnimState();
    }

    public void SetSize(int size)
    {
        if (growingUpController != null)
        {
            growingUpController.Size = size;
        }
    }

   
    public void ActRagdollCollision(Collision collision)
    {
        if(gameObject.tag != "Collision")
        {
            return;
        }

        if (levelController.GetLevelState == LevelController.LevelState.Battle)
        {
            return;
        }

        var tempStickman = collision.gameObject.GetComponentInParent<PlayerStickmanController>();

        if (tempStickman == null || velocityBeforePhysicsUpdate.sqrMagnitude <= tempStickman.velocityBeforePhysicsUpdate.sqrMagnitude)
        {
            return;
        }

        if(tempStickman.gameObject == gameObject)
        {
            return;
        }

        if (Size == tempStickman.Size)
        {
           
            Vector3 force = Vector3.forward * velocityBeforePhysicsUpdate.magnitude * generalConfig.CollisionImpulse + Vector3.up * generalConfig.UpCollisionImpulse;

            tempStickman.growingUpController.GrowingUpStickman(this);
            

            //tempStickman.ActivateRagdoll();

            tempStickman.spineRigidBody.AddForce(force);
            
            //tempStickman.StartCoroutine(tempStickman.StickmanWakeUp());

            if (tempStickman.Size > 0)
            {
                GameObject effect = Instantiate(config.PlayerStickmansMergeEffect, collision.contacts[0].point, Quaternion.identity, null);
                Destroy(effect, 2);
            }
            gameObject.tag = "NoCollision";
            StartCoroutine(SetCollisionAgain());
           
            Destroy(gameObject);
            EventManager.TriggerEvent(DeathEvents.Death_stickman, damageHpManager);  
        }

        else
        {
            if (isControlled || tempStickman.isControlled)
            {
                GameObject effect = Instantiate(generalConfig.NoMergeEffect, collision.contacts[0].point, Quaternion.identity, null);
                Destroy(effect, 2);


                Vector3 force = -collision.contacts[0].normal * velocityBeforePhysicsUpdate.magnitude * generalConfig.CollisionImpulse;

                ActivateRagdoll();
                spineRigidBody.AddForce(-force / 2f);

                animator.SetBool("Run", false);
                StartCoroutine(StickmanWakeUp());

                tempStickman.ActivateRagdoll();
                tempStickman.StartCoroutine(tempStickman.StickmanWakeUp());

                tempStickman.spineRigidBody.AddForce(force);
            }
        }

    }

    private IEnumerator SetCollisionAgain()
    {
        yield return null;
        gameObject.tag = "Collision";
    }
    private void OnCollisionEnter(Collision collision)
    {
        ActRagdollCollision(collision);

        if(collision.gameObject.tag == "WallBetweenEnemy" && isControlled)
        {
            SetIsControlledStickman(false);
            animator.SetBool("Run", false);
        }
    }

    public void AddBounce(ArmorBounce armorBounce)
    {
        GameObject armor = null;
        switch ((int)armorBounce.GetTypeArmor)
        {
            case 0:
                armor = Shield.gameObject;
                break;
            case 1:
                armor = sword.gameObject;
                battleController.ChangeAnimationAttack(global::BattleController.PunchingState.Sword);
                break;
            case 2:
                armor = shelmet.gameObject;
                break;
        }

        if (armor != null)
        {
            armor.SetActive(true);
            //armor.GetComponentInChildren<MeshRenderer>().material = armorBounce.gameObject.GetComponentInChildren<MeshRenderer>()?.material;
        }

        damageHpManager.HP += armorBounce.HpBounce;
        damageHpManager.Damage += armorBounce.DamageBounce;
        UpdateUI();

    }
   
    private void ActivateRagdoll()
    {
        if (isControlled)
        {
            SetIsControlledStickman(false);
            animator.SetBool("Run", false);
        }
        SetEnabled(true);
        GetComponent<Collider>().enabled = false;
        ragdollHelper.ragdolled = true;
    }

    public void SetRunAnimState()
    {
        animator.SetBool("Run", true);
    }

    public void SetLayer(string nameLayer)
    {
        foreach (Transform childTransform in GetComponentsInChildren<Transform>())
        {
            childTransform.gameObject.layer = LayerMask.NameToLayer(nameLayer);
        }
    }
    public void SetIsControlledStickman(bool isControlled)
    {
        if (isControlled)
        {
            SetLayer("StartStickman");

            AddRigidBodyOnParent();
        }
        else
        {
            SetLayer("StartStickman");

            RemoveRigidBodyOnParent();
        }
        this.isControlled = isControlled;
    }

    private void RemoveRigidBodyOnParent()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            return;
        }
        Destroy(GetComponent<Rigidbody>());
        GetComponent<Collider>().enabled = false;
        rigidBody = spineRigidBody;
        SetEnabled(true);
    }

    private void AddRigidBodyOnParent()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            rigidBody = gameObject.AddComponent<Rigidbody>();
        }

        SetEnabled(false);
        rigidBody.isKinematic = true;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        GetComponent<Collider>().enabled = true;
    }

    private void OnDead(DamageHPManager tempDamageHpManager)
    {
        if(damageHpManager == tempDamageHpManager)
        {
            SetLayer("InActiveStickman");
            RemoveRigidBodyOnParent();
            foreach (Renderer renderer in GetComponentsInChildren<MeshRenderer>())
            {
                renderer.material = generalConfig.WhiteMaterial;
            }
            foreach (Renderer renderer in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                Material[] materials = new Material[renderer.materials.Length];
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = generalConfig.WhiteMaterial;
                }

                renderer.materials = materials;
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
        Destroy(gameObject);
    }
 
    private IEnumerator StickmanWakeUp()
    {
        yield return new WaitForSeconds(generalConfig.TimeToRagdollUp);
        ragdollHelper.ragdolled = false;
      //  AddRigidBodyOnParent();
    }

    public void StepBack()
    {
        SetAnimTrigger("StepBack", animator);
    }

    protected void SetAnimTrigger(string triggerName, Animator animator)
    {
        foreach (AnimatorControllerParameter p in animator.parameters)
            if (p.type == AnimatorControllerParameterType.Trigger)
                animator.ResetTrigger(p.name);
        animator.SetTrigger(triggerName);
    }
}
