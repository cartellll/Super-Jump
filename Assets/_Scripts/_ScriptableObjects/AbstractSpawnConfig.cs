using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSpawnConfig : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    public GameObject Prefab => prefab;
  //  public GameObject Prefab { get; set; }
}
