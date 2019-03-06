using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 物体复原 调用ReProduct 物体将返回到指定的位置
 */
public class PreduceObject : MonoBehaviour {

    public Transform reProductTrans;
    public float delayReproductTime = 0f;

    public void ReProduct()
    {
        if (reProductTrans != null)
        {
            StartCoroutine(Delay());
        }
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delayReproductTime);
        transform.rotation = reProductTrans.rotation;
        transform.position = reProductTrans.position;
    }
}
