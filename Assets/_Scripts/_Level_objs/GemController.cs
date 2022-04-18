using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperStars.Events;
public class GemController : MonoBehaviour
{

    [SerializeField] private GemConfig config;

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0, config.SpeedRotation * Time.deltaTime, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        var playerStickman = other.gameObject.GetComponentInParent<PlayerStickmanController>();
        if (playerStickman != null)
        {
            EventManager.TriggerEvent(MoneyEvents.AddMoney, 1);
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
