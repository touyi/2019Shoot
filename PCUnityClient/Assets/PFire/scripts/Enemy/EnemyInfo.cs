using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyState
{
    Normal,
    Faint
};
public class EnemyInfo : MonoBehaviour {

    public EnemyState state = EnemyState.Normal;
    public float power = 10f;
    public float healthy = 100f;
    public float defend = 10;
    public float flyspeed = 5f;
    public float WeakProportion = 1.5f;
    public bool isDeath = false;
    
    // todo
}
