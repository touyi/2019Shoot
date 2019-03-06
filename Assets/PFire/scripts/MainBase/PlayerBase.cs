using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        EnemyInfo info = other.gameObject.GetComponent<EnemyInfo>();
        if (info != null)
        {
            FGameInfo.Instance.PlayerHealthy -= info.power;
            FGameObjectBar._instance.shakeCamera.Shake();
            EnemyContorl ec = other.gameObject.GetComponent<EnemyContorl>();
            // ec.GetDamage(info.healthy);
            ec.DestroyPlane();
        }
    }


}
