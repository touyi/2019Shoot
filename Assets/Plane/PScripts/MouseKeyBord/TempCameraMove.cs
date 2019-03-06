using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 远程控制陀螺仪移动
 * 因为通行不连续原因，需要对陀螺仪数据插值计算，考虑360-0是相等的角度，插值会出问题，所以手写插值
 */
public class TempCameraMove : MonoBehaviour {

    //Vector3 nowRota;
    Vector3 lastRota;
    public Vector3 offSet;
    bool isFirst = true;
	void Update () {
        /*Vector3 touch = new Vector3(-Input.GetAxis("Mouse Y"),
        Input.GetAxis("Mouse X"),0);
        transform.Rotate(touch);*/
    }
    float ShortertPath(float num1, float num2, float min = 0, float max = 360, float t = 0.1f)
    {
        float len1 = Mathf.Abs(num1 - num2);
        float len2 = Mathf.Abs((max - min) - len1);
        float minnum = Mathf.Min(num1, num2);
        float maxnum = Mathf.Max(num1, num2);
        if (len1 < len2)
        {
            if(num1 > num2)
            {
                num1 -= len1 * t;
            }
            else
            {
                num1 += len1 * t;
            }
            return num1;
        }
        else
        {
            if(num1>num2)
            {
                num1 += len2 * t;
            }
            else
            {
                num1 -= len2 * t;
            }
            return SetRange(num1);
        }
    }
    float SetRange(float num)
    {
        while (num < 0) num += 360;
        while (num > 360) num -= 360;
        return num;
    }
}
