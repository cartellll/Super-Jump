using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperStars.Events;

public class DamageHPManager : MonoBehaviour
{
   
    [SerializeField] private DamageHPConfig config;
    public DamageHPConfig Config => config;
    [SerializeField] private Transform deathEffectTransform;
    public Transform DeathEffectTransform => deathEffectTransform;

    [SerializeField] private Transform hitEffectTransform;
    public Transform HitEffectTransform => hitEffectTransform;

    private float hp;
    public float HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if (uiController != null)
            {
                uiController.UpdateText(hp);
            }
            if (hp <= 0)
            {
                EventManager.TriggerEvent(DeathEvents.Death_stickman, this);
                uiController.Text.gameObject.SetActive(false);
               // Destroy(gameObject);
            }
        }
    }

    private float damage;
    public float Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    [Header("UI")]
    [SerializeField] private StickmanUIController uiController;

    void Start()
    {
        if (GetComponent<PlayerStickmanController>() == null)
        {
            hp = config.MaxHP;
            damage = config.Damage;
        }
    }

    public void ActivateDeathEffect()
    {
        if(config.DeathEffect == null)
        {
            return;
        }

        Vector3 deathPosition = gameObject.transform.position;
        if (deathEffectTransform != null)
        {
            deathPosition = deathEffectTransform.position;
        }
        Instantiate(Config.DeathEffect, deathPosition,Quaternion.identity, gameObject.transform);
    }

    public void ActivateHitEffect()
    {
        if (config.HitEffect == null)
        {
            return;
        }

        Vector3 hitPosition = gameObject.transform.position;
        if (hitEffectTransform != null)
        {
            hitPosition = hitEffectTransform.position;
        }
        Instantiate(Config.HitEffect, hitPosition, Quaternion.identity, gameObject.transform);
    }
}
