using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCountListen : MonoBehaviour {

    public FireBullet fireBullet;
    public TextMesh tmesh;
    void Update()
    {
        tmesh.text = fireBullet.BulletCount.ToString() + "/" + fireBullet.MaxBulletCount.ToString();
    }
}
