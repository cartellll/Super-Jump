using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera MyCamera;

    private void Awake()
    {
        if (MyCamera == null) MyCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + Vector3.back);
    }
}