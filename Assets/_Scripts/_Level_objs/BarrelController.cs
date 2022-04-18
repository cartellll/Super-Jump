using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BarrelController : MonoBehaviour
{
    [SerializeField] private BarrelConfig config;
    [SerializeField] private GameObject fireEffectObject;
    private enum State
    {
        Stay,
        Move
    }
    private Rigidbody rb;
    State state = State.Stay;
    // Update is called once per frame
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (state == State.Move)
        {
            transform.rotation *= Quaternion.Euler(0, config.SpeedRotation * Time.deltaTime, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerStickman = other.gameObject.GetComponentInParent<PlayerStickmanController>();
        if (playerStickman != null)
        {
            playerStickman.SetLayer("InActiveStickman");
            playerStickman.DamageHpManager.HP = 0;
            state = State.Move;
            rb.velocity = Vector3.forward * config.SpeedMove;
        }

        var enemyStickman = other.gameObject.GetComponentInParent<Enemy>();
        if (enemyStickman != null)
        {
            enemyStickman.DamageHpManager.HP = 0;
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
