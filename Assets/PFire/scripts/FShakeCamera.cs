using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 震动脚本
 * 摄像机抖屏
 */
public class FShakeCamera : MonoBehaviour {

    Vector3 pos;
    public float lastedTime;
    float countTime;
    public float range;
    float unit;
    private void Start()
    {
        pos = transform.position;
    }
    void Update () {
        if (countTime > 0)
        {
            transform.position = pos + Random.insideUnitSphere * unit * range;
            countTime -= Time.deltaTime;
            unit = countTime * lastedTime;
        }
        else transform.position = pos;
	}

    public void Shake(float ltime = 1)
    {
        lastedTime = ltime;
        countTime = lastedTime;
        unit = 1;
    }
}
