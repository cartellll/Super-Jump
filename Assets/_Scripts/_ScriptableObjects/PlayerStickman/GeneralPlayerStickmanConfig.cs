using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "General Stickman Config", menuName = "Configs/General Stickman Config")]
public class GeneralPlayerStickmanConfig : ScriptableObject
{

    [SerializeField] private Material whiteMaterial;
    public Material WhiteMaterial => whiteMaterial;
    [Min(0)]
    [SerializeField] private float timeToRagdollUp;
    public float TimeToRagdollUp => timeToRagdollUp;

    [Min(0)]
    [SerializeField] private float collisionImpulse;
    public float CollisionImpulse => collisionImpulse;

    [SerializeField] private float upCollisionImpulse;
    public float UpCollisionImpulse => upCollisionImpulse;

    [SerializeField] private GameObject noMergeEffect;
    public GameObject NoMergeEffect => noMergeEffect;
}
