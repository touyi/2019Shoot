using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCSled : MonoBehaviour {
    
    public float timer = 0.3f;
    public void Show()
    { 
        MITween.Tween(this.gameObject, new Vector3(1, 1, 1), timer);
    }
    public void Hidden()
    {
        MITween.Tween(this.gameObject, new Vector3(1, 0, 0), timer);
    }
}
