using UnityEngine;

namespace GamePool.OBJ
{
    public class GPSpark : GPObject
    {
        public float timer;
        public static string ResName()
        {
            return "prefabs/effect/vulcan_spark";
        }
        public override void Awake()
        {
        }

        public override void Destory()
        {
        }
        void Update()
        {
            timer -= Time.deltaTime;
            if(timer<=0)
            {
                GPGameObjectPool.Return(this.gameObject);
            }
        }
        public override void Start()
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            ps.Play();
            timer = ps.main.duration;
        }
    }
}
