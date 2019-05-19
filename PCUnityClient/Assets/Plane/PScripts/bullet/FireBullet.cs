using System.Collections;
using System.Collections.Generic;
using GamePool.OBJ;
using UnityEngine;
/*
 * 普通火枪子弹
 * 子弹数量计数
 */
public class FireBullet : CreatBullet {
    private int maxBulletCount = 60;
    public float exportBulletTime = 5f;
    int bulletCount = 60;

    public int BulletCount
    {
        get
        {
            return bulletCount;
        }
    }

    public int MaxBulletCount
    {
        get
        {
            return maxBulletCount;
        }

        set
        {
            maxBulletCount = value;
            bulletCount = maxBulletCount;
        }
    }

    public override void Fire()
    {
        if(isUse && Timer._instance.regist(gameObject.name, cd))
        {
            if (bulletCount > 0)
            {
                int id = Random.Range(0, fireTrans.Length);
                bulletCount--;
                if(bulletCount<=0)
                {
                    FGameObjectBar._instance.StartCoroutine(ExportBullet(exportBulletTime));
                }
                GPGameObjectPool.Creat<GPFireBullet>(fireTrans[id].position, fireTrans[id].rotation);
            }
        }
    }

    IEnumerator ExportBullet(float delay)
    {
        yield return new WaitForSeconds(delay);
        bulletCount = MaxBulletCount;
    }

}
