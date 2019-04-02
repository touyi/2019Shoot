using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaintPlaneBar : CreatBullet
{
    public SliderControl scontrol;
    public override void Fire()
    {
        if(!Timer._instance.regist(this.name, cd))
        {
            return;
        }
        EnemyContorl[] enemys = GameObject.FindObjectsOfType<EnemyContorl>();
        foreach(EnemyContorl it in enemys)
        {
            it.FaintPlane();
        }
    }

    void Update()
    {
        scontrol.Value = 1-(Timer._instance.GetRemainTime(this.name)/cd);
    }
}
