using UnityEngine;

namespace GamePool.OBJ
{
    public class GPFireBullet : GPObject
    {
        public float lifetime = 5f;
        public float velocity = 300f;
        public float power = 20f;
        //public LayerMask layermask;
        float raycastAdvance = 2f;
        RaycastHit hitpoint;
        bool ishit = false;
        float timer = 0f;
        void ApplyForce(float force)
        {
            // 对物体射击点进行加力
            if(hitpoint.rigidbody!=null)
            {
                hitpoint.rigidbody.AddForceAtPosition(transform.forward * force, hitpoint.point, ForceMode.VelocityChange);
            }
        }
        public static string ResName()
        {
            return "prefabs/bullet/vulcan_projectile";
        }
        public override void Awake()
        {
       
        }
        void Update()
        {
            Vector3 step = transform.forward * Time.deltaTime * velocity;
            timer += Time.deltaTime;
            if(Physics.Raycast(transform.position,transform.forward,out hitpoint,step.magnitude * raycastAdvance))
            {
                // 发射爆炸特效
                GPGameObjectPool.Creat<GPSpark>(hitpoint.point, transform.rotation);
                // 添加撞击力量
                ApplyForce(2.5f);
                WhenAttackObject(hitpoint.transform.gameObject);
                // 销毁自身
                GPGameObjectPool.Return(this.gameObject);
            }
            else
            {
                transform.position += step;
                if(timer >=  lifetime)
                {
                    GPGameObjectPool.Return(this.gameObject);
                }
            }
        }
        public override void Destory()
        {
        }
        public override void Start()
        {
            ishit = false;
            hitpoint = new RaycastHit();
            timer = 0f;
        }
        public virtual void WhenAttackObject(GameObject attackgo)
        {
            EnemyContorl enemy = attackgo.GetComponent<EnemyContorl>();
            if(enemy!=null)
            {
                enemy.GetDamage(power);
            }
        }
    }
}
