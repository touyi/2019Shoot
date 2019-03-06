using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneDestoryMessage : MonoBehaviour {
    public EnemySpawn eSpawn;
	void OnDestroy()
    {
        if(eSpawn != null)
            eSpawn.PlaneDestory(this.gameObject);
    }
}
