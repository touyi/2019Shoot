using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotationControl : MonoBehaviour {

    Transform gunBase;
    Vector3 screenPos;
    Vector3 lastScreenPos;
    Queue<Vector3> smoothQue = new Queue<Vector3>();
    public float speed = 0.03f;
    private void Start()
    {
        screenPos.y = Screen.height / 2f;
        screenPos.x = Screen.width / 2f;
        screenPos.z = 2000f;
        lastScreenPos = screenPos;
        gunBase = FGameObjectBar._instance.gunBase.transform;
    }
    public void SetScreenPos(Vector2 pos)
    {
        pos /= 9.8f;
        float angle = Mathf.Sqrt(Mathf.Pow(pos.y, 2) + Mathf.Pow(pos.x, 2));
        if (angle > 1)
            angle = 1;
        float len = Mathf.PI / 2f - Mathf.Acos(angle);
        pos *= len;

        
        screenPos.x = (-pos.x+1)*Screen.width/2;
        screenPos.y = (-pos.y+1)*Screen.height/2;
        screenPos.z = 2000f;



        screenPos = GetLimitPos(screenPos);
        smoothQue.Enqueue(screenPos);
        while (smoothQue.Count > 8) smoothQue.Dequeue();
    }
    private void Update()
    {
        //SetScreenPos(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        // screenPos = GetLimitPos(screenPos);
        // lastScreenPos = Vector3.Slerp(lastScreenPos, screenPos, speed);
        // lastScreenPos = GetLimitPos(lastScreenPos);

        // Vector3 worldPos = FGameObjectBar._instance.shakeCamera.GetComponent<Camera>().ScreenToWorldPoint(lastScreenPos);
        // 算术平均滤波+一阶滞后滤波法
        Vector3 avg = new Vector3(0, 0, 0);
        foreach (Vector3 it in smoothQue)
        {
            avg += it;
        }
        if(smoothQue.Count>0)
            avg /= smoothQue.Count;

        lastScreenPos = Vector3.Slerp(lastScreenPos, avg, speed);
        Vector3 worldPos = FGameObjectBar._instance.shakeCamera.GetComponent<Camera>().ScreenToWorldPoint(lastScreenPos);
        gunBase.LookAt(worldPos);
    }
    Vector3 GetLimitPos(Vector3 pos)
    {
        if (pos.x <= 0)
            pos.x = 0;
        if (pos.y <= 0)
            pos.y = 0;
        if (pos.x >= Screen.width)
            pos.x = Screen.width - 1;
        if (pos.y >= Screen.height)
            pos.y = Screen.height - 1;
       // Debug.Log(pos);
        return pos;
    }
}
