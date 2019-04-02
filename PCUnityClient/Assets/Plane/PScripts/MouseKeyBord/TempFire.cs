using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempFire : MonoBehaviour {

    public CreatBullet[] fire;
    int id = 0;        // 当前武器编号
    public float rotaSpeed = 0.8f;
    public delegate void OnLister(GameObject go);
    public event OnLister OnWeaponChange;
    // Use this for initialization
    void Start () {
        /*fire = GetComponentInChildren<CreatBullet>();
        fire.SetGunUse(true);
        fire.gameObject.GetComponent<BoxCollider>().enabled = false;
        fire.gameObject.GetComponent<Rigidbody>().useGravity = false;*/
        BoxCollider box;
        Rigidbody rigi;
        foreach(CreatBullet it in fire)
        {
            
            it.gameObject.SetActive(false);
            box = it.gameObject.GetComponent<BoxCollider>();
            if (box != null) box.enabled = false;
            rigi = it.gameObject.GetComponent<Rigidbody>();
            if (rigi != null) rigi.useGravity = false;
        }
        ChangeWeapon(0);
	}
	void ChangeWeapon(int _id = -1)
    {
        if(_id==-1)
        {
            fire[id].End();
            fire[id].gameObject.SetActive(false);
            id++;
            id %= fire.Length;
            fire[id].begin();
            fire[id].gameObject.SetActive(true);
        }
        else
        {
            fire[id].End();
            fire[id].gameObject.SetActive(false);
            id = _id;
            fire[id].begin();
            fire[id].gameObject.SetActive(true);
        }
        Vector3 cRot = transform.eulerAngles;
        //transform.localEulerAngles = new Vector3(60, 60, 0);
        // 切枪动画
        fire[id].transform.localEulerAngles = new Vector3(60, 60, 0);
        ChangeAnimotion(fire[id].gameObject);

        if(OnWeaponChange!=null)
        {
            OnWeaponChange(fire[id].gameObject);
        }
    }
    void ChangeAnimotion(GameObject go)
    {
        Hashtable args = new Hashtable();
        args.Add("rotation", new Vector3(0, 0, 0));
        args.Add("islocal", true);
        args.Add("time", 0.2f);
        args.Add("easeType", iTween.EaseType.linear);
        iTween.RotateTo(go, args);
    }
	// Update is called once per frame
	void Update () {
        if (PInput.Fire())
        {
            fire[id].Fire();
        }
       
        if (PInput.GetChangeButton())
        {
            ChangeWeapon();
        }
        //transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), rotaSpeed);
	}
}
