using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    [SerializeField] private Transform leftPlayerLimit;
    [SerializeField] private Transform rightPlayerLimit;
    [SerializeField] private Transform instantinatePosition;
    [SerializeField] private GameObject wallBetweenEnemyAndPlayer;
    [SerializeField] private GameObject enemyAttackTrigger;
    [SerializeField] private Transform cameraBattleTransform;
    [SerializeField] private RopeConstruction ropeContruction;

    public Transform InstantinatePosition => instantinatePosition;
    public Transform LeftPlayerLimit => leftPlayerLimit;
    public Transform RightPlayerLimit => rightPlayerLimit;
    public GameObject WallBetweenEnemyAndPlayer => wallBetweenEnemyAndPlayer;
    public GameObject EnemyAttackTrigger => enemyAttackTrigger;
    public Transform CameraBattleTransform => cameraBattleTransform;
    public RopeConstruction GetRopeConstruction => ropeContruction;

    [System.Serializable]
    public struct RopeConstruction
    {
        public Transform LeftCylinder;
        public Transform RightCyilinder;
    }
}
