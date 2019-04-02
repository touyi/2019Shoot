using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class blueline : MonoBehaviour {

    public float MaxBeamLength = 1000;
    public float beamscale = 0.5f;
    public float BeamShootSpeed = -6f;
    public Transform startEffect;
    public Transform exploreEffect;
    public float power = 200;
    RaycastHit hitpoint;
    LineRenderer linereder = null;
    float beamlength;

    private void Awake()
    {
        linereder = GetComponent<LineRenderer>();
        linereder.SetPosition(0, Vector3.zero);
        hitpoint = new RaycastHit();
    }
    void ApplyForce(float force)
    {
        // 对物体射击点进行加力
        if (hitpoint.rigidbody != null)
        {
            hitpoint.rigidbody.AddForceAtPosition(transform.forward * force, hitpoint.point, ForceMode.VelocityChange);
        }
    }
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitpoint, MaxBeamLength))
        {
            beamlength = Vector3.Distance(transform.position, hitpoint.point);
            // 攻击到物体
            WhenAttackObject(hitpoint.transform.gameObject);
            ApplyForce(5f);
        }
        else
            beamlength = MaxBeamLength;

        if(exploreEffect!=null)
        exploreEffect.position = transform.position + transform.forward * beamlength; 
        linereder.material.SetTextureScale("_MainTex", new Vector2(beamscale / 10f * beamlength, 1f));
        linereder.SetPosition(1, new Vector3(0f,0f,beamlength));
        linereder.material.SetTextureOffset("_MainTex", new Vector2(Time.time * BeamShootSpeed, 0f));
    }
    public virtual void WhenAttackObject(GameObject attackgo)
    {
        EnemyContorl enemy = attackgo.GetComponent<EnemyContorl>();
        if(enemy!=null)
        {
            enemy.GetDamage(power*Time.deltaTime);
        }
    }
}
