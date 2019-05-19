using System.Collections;
using System.Collections.Generic;
using GamePool.OBJ;
using UnityEngine;
public delegate void VoidDelgate(GameObject go);
public class EnemyContorl : MonoBehaviour {
    public VoidDelgate WhenDestory;
    public EnemyInfo info;
    public Transform fireTarget;
    public float damping; // 转向速度
    const float delayDestory = 0.1f;
    const float playerAddhealthySpeed = 20f;
    float faintTimeCd;
    void Start()
    {
        // 设定攻击目标
        fireTarget = FGameObjectBar._instance.myBase.transform;
    }
    void Update()
    {
        switch(info.state)
        {
            case EnemyState.Normal:
                UpdateNormal();
                break;
            case EnemyState.Faint:
                UpdateFaint();
                break;
        }
    }
    protected virtual void UpdateNormal()
    {
        if (fireTarget != null) // 平滑转向
        {
            Quaternion rota = Quaternion.LookRotation(fireTarget.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rota, Time.deltaTime * damping);
        }
        transform.position += transform.forward * info.flyspeed * Time.deltaTime;
    }
    protected virtual void UpdateFaint()  // 当飞机收到干扰的时候运行
    {
        faintTimeCd -= Time.deltaTime;
        if(faintTimeCd<=0)
        {
            info.state = EnemyState.Normal;
        }
        if (fireTarget != null) // 平滑转向
        {
            Quaternion rota = Quaternion.LookRotation(fireTarget.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rota, Time.deltaTime * damping);
        }
        transform.position += transform.forward * info.flyspeed * Time.deltaTime * 0.1f;
    }
    public virtual void GetDamage(float damage)
    {
        
        if(info.state==EnemyState.Faint) // 收到干扰时机体防御降低 收到伤害扩大一定比例
        {
            damage *= info.WeakProportion;
        }
        info.healthy -= damage;
        if(info.healthy<=0)
        {
            // 消灭敌机加血 50
            FGameInfo.Instance.PlayerHealthy += playerAddhealthySpeed;
            FGameInfo.Instance.NowScore += 100;
            DestroyPlane();
        }
    }
    public void DestroyPlane()
    {
        info.isDeath = true;
        WhenPlaneDeath();
        if (WhenDestory != null)
        {
            WhenDestory(this.gameObject);
        }
        GetComponent<Collider>().enabled = false;
        Destroy(this.gameObject, delayDestory);
    }
    protected virtual void WhenPlaneDeath() // 当飞机死亡时调用
    {
        GPGameObjectPool.Creat<GPExplosion>(transform.position, transform.rotation);
    }
    public virtual void FaintPlane(bool faint = true)
    {
        info.state = EnemyState.Faint;
        faintTimeCd = 5;
    }
}
