using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointType
{
    Boat,
    Horse
}

public class PointController : MonoBehaviour
{
    [SerializeField] private AbstractSpawnConfig config;
    private LevelController currentLevel;
    private void Start()
    {
        currentLevel = GameManager.Instance.LevelManager.CurrentLevel;
        GameObject gameObject = Instantiate(config.Prefab, transform.position, transform.rotation, currentLevel.transform);
        
        var enemy = gameObject.GetComponent<Enemy>();
        if(enemy!= null)
        {
            currentLevel.EnemyCrowd.AddStickman(enemy.DamageHpManager);
            enemy.Config = (EnemyConfig) config;
        }

        var playerStickman = gameObject.GetComponent<PlayerStickmanController>();
        if (playerStickman != null)
        {
            currentLevel.PlayerCrowd.AddStickman(playerStickman.DamageHpManager);
            playerStickman.Config = (PlayerStickmanConfig)config;
            playerStickman.UpdateParametrs();
        }

    }
}