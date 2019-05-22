using System.Collections;
using Tools;
using UnityEngine;

namespace GamePool.OBJ
{
    public class GPExplosion : GPObject
    {
        public static string ResName()
        {
            return StringDefine.ExplosionPath;
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
            GetComponent<ParticleSystem>().Play();
        }
        IEnumerator DelayDestory(float timer)
        {
            yield return new WaitForSeconds(timer);
            GPGameObjectPool.Return(this.gameObject);
        }
    }
}
