using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScaleView : MonoBehaviour {
    [SerializeField]
    float enLarge;
    float distance;
    Vector3 initScale;
    private void Start()
    {
        initScale = transform.localScale;
        distance = Vector3.Distance(transform.position, FGameObjectBar._instance.myBase.transform.position);
        transform.localScale = initScale * (enLarge+1f);
    }
    private void Update()
    {
        float pray = Vector3.Distance(transform.position, FGameObjectBar._instance.myBase.transform.position) / distance;
        transform.localScale = initScale * (enLarge*pray+1f);
        
    }
}
