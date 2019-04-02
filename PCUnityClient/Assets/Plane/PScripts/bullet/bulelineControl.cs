using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 蓝色激光光束
 * 激光使用能量
 * 激活激光后一定时间（极短）不开火激光熄灭
 */
public class bulelineControl : CreatBullet {
    public float delayTime = 0.1f;
    GameObject bluelinebullet = null;
    private float maxEnergy = 100f;
    public float EnergyUseSpeed = 10f;
    float lineEnergy = 100f;
    float timer = 0f;
    

    public float LineEnergy
    {
        get
        {
            return lineEnergy;
        }
    }

    public float MaxEnergy
    {
        get
        {
            return maxEnergy;
        }

        set
        {
            maxEnergy = value;
            lineEnergy = maxEnergy;
        }
    }

    void Awake()
    {
        GameObject go = Resources.Load("prefabs/bullet/plasma_beam_blue") as GameObject;
        bluelinebullet = Instantiate(go, transform);
        bluelinebullet.SetActive(false);
    }
    public override void Fire()
    {
        if (!isUse) return;
        if (!bluelinebullet.gameObject.activeInHierarchy && lineEnergy/MaxEnergy>0.1f)
        {
            bluelinebullet.gameObject.SetActive(true);
        }
        timer = delayTime;
        int id = Random.Range(0, fireTrans.Length);
        bluelinebullet.transform.position = fireTrans[id].position;
        bluelinebullet.transform.rotation = fireTrans[id].rotation;
    }
    void Update()
    {
        if (bluelinebullet.activeInHierarchy)
        {
            timer -= Time.deltaTime;
            lineEnergy -= Time.deltaTime * EnergyUseSpeed;
        }
        else
        {
            lineEnergy += Time.deltaTime * EnergyUseSpeed;
            lineEnergy = Mathf.Min(lineEnergy, MaxEnergy);
        }
        if(timer<=0 || lineEnergy<=0)
        {
            bluelinebullet.gameObject.SetActive(false);
        }
    }
}
