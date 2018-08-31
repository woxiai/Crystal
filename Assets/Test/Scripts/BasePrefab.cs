using System;
using System.Collections.Generic;
using UnityEngine;

public class BasePrefab : MonoBehaviour
{

    public float speed = 0.5F;

    protected Transform trans;

    protected virtual void Awake()
    {
        trans = transform;
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        trans.Rotate(Vector3.up * speed);
    }
}

