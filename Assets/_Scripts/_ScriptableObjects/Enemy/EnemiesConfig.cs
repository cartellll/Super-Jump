﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EnemiesConfig", menuName = "Configs/Enemies Config")]
public class EnemiesConfig : ScriptableObject
{
    [SerializeField] private EnemyConfig[] enemyStickmanConfigs;
    public EnemyConfig[] EnemyStickmanConfigs => enemyStickmanConfigs;

    /*   void OnValidate()
       {
           if (numberOfSizes!= 0 && playerStickmanMaterials.Length != numberOfSizes)
           {
               Debug.LogWarning("Don't change the 'ints' field's array size!");
               Array.Resize(ref playerStickmanMaterials, numberOfSizes);
               Array.Resize(ref playerStickmanScales, numberOfSizes);
               Array.Resize(ref playerStickmanSpeeds, numberOfSizes);
               Array.Resize(ref playerStickmanHP, numberOfSizes);
               Array.Resize(ref playerStickmanDamage, numberOfSizes);
               Array.Resize(ref playerStickmanMergeEffect, numberOfSizes-1);
           }
       }*/
}
