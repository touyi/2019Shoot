﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPExplosion : GPObject
{
    public static string ResName()
    {
        return "prefabs/effect/Explosion_001";
    }
    public override void Awake()
    {
        StartCoroutine(DelayDestory(GetComponent<ParticleSystem>().main.duration-0.1f));
    }

    public override void Destory()
    {
    }

    public override void Start()
    {
    }
    IEnumerator DelayDestory(float timer)
    {
        yield return new WaitForSeconds(timer);
        GPGameObjectPool.Return(this.gameObject);
    }
}
