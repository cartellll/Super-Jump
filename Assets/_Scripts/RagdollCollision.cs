using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollCollision : MonoBehaviour
{
    private PlayerStickmanController parentStickman;
    void Awake()
    {
        parentStickman = GetComponentInParent<PlayerStickmanController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        parentStickman.ActRagdollCollision(collision);
    }
}
