using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAim : MonoBehaviour {

    LineRenderer aimLine;
    Transform aimPos;
    bool isHaveAim = false;
    [SerializeField]
    float sWidth;
    [SerializeField]
    float eWidth;
    [SerializeField]
    float Length;
    private void Awake()
    {
        GetComponent<TempFire>().OnWeaponChange += GetWeaponAim;
    }
    void GetWeaponAim(GameObject go)
    {
        Transform aim = go.transform.Find("aim");
        if(aim==null)
        {
            isHaveAim = false;
            return;
        }
        isHaveAim = true;
        SetLineInf(aim);
    }
    public void SetLineInf(Transform sp)
    {
        aimPos = sp;
    }
    private void Start()
    {
        aimLine = GetComponent<LineRenderer>();
        aimLine.startWidth = sWidth;
        aimLine.endWidth = eWidth;
    }
    private void Update()
    {
        if (isHaveAim)
        {
            aimLine.SetPosition(0, aimPos.position);
            aimLine.SetPosition(1, aimPos.position+aimPos.forward * Length);
            
        }
        else
        {
            aimLine.SetPosition(0, Vector3.zero);
            aimLine.SetPosition(1, Vector3.zero);
        }
    }
}
