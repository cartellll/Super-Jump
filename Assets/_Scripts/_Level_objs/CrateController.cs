using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperStars.Events;
public class CrateController : MonoBehaviour
{

    [SerializeField] private CrateConfig config;


    private void OnCollisionEnter(Collision collision)
    {
        var playerStickman = collision.gameObject.GetComponentInParent<PlayerStickmanController>();
        if (playerStickman != null)
        {
            playerStickman.SetLayer("InActiveStickman");
            playerStickman.DamageHpManager.HP = 0;
            Die();
        }
    }

    public void Die()
    {
        if (config.DeathEffectPrefab != null)
        {
            var deathEffect = Instantiate(config.DeathEffectPrefab, transform.position, Quaternion.identity, null);
            Destroy(deathEffect, 3);
        }

        Destroy(gameObject);
    }
}
