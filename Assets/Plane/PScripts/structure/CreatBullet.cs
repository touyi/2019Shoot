using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 弹夹基类
 * 所有的子弹继承该类
 */
public abstract class CreatBullet:MonoBehaviour {

    public float cd = 0.3f;
    public Transform[] fireTrans;
    protected bool isUse = false;
    // 将枪的激活（begin）失效（end）开火（Fire）与弹夹绑定
    void Start()
    {
        InterfaceGun igun = GetComponent<InterfaceGun>();
        if (igun != null)
        {
            igun.GunBegin += begin;
            igun.GunEnd += End;
            igun.Fire += Fire;
        }
    }
    public void SetGunUse(bool use)
    {
        isUse = use;
    }
    public virtual void begin()
    {
        isUse = true;
    }
    public virtual void End()
    {
        isUse = false;
    }
    public abstract void Fire();
}

