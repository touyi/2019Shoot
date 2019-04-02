using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 战斗跟随显示屏
 */
public class PCScreen : MonoBehaviour {
    [SerializeField]
    PCBar bar;
    [SerializeField]
    PCSled led;
    [SerializeField]
    float length = 10;
    [SerializeField]
    float Offsetleft;
    [SerializeField]
    float followSpeed = 0.02f;
    public Transform target;
    void Update()
    {
       if(target!=null)
        {
            Vector3 vec = -(target.position - transform.position);
            transform.rotation = Quaternion.LookRotation(vec);

            vec = target.forward-target.right*Offsetleft;
            vec.y = 0;
            vec = vec.normalized;
            vec *= length;
            vec += target.position;
            vec.y = transform.position.y;
            transform.position = Vector3.Slerp(transform.position,vec,followSpeed);
            

        }
    }
    public void ShowScreen()
    {
        bar.Show();
        StartCoroutine(Delay(bar.timer, led.Show));
    }
    public void HiddenScreen()
    {
        led.Hidden();
        StartCoroutine(Delay(led.timer, bar.Hidden));
    }
    IEnumerator Delay(float timer,DelegateMethod callback)
    {
        yield return new WaitForSeconds(timer);
        callback();
    }
}
