using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    static public Timer _instance = null;
    Dictionary<string, float> timedic = new Dictionary<string, float>();
    void Awake()
    {
        _instance = this;
    }
    public bool regist(string name, float cd)
    {
        if (timedic.ContainsKey(name))
        {
            return false;
        }
        timedic[name] = cd + Time.time;
        return true;
    }
    public void Unregist(string name)
    {
        if (timedic.ContainsKey(name))
        {
            timedic.Remove(name);
        }
    }
    public bool isContain(string name)
    {
        return timedic.ContainsKey(name);
    }
    public float GetRemainTime(string name)
    {
        if(timedic.ContainsKey(name))
        {
            return timedic[name] - Time.time;
        }
        return 0;
    }
    void Update()
    {
        //Debug.Log(OVRInput.Get(OVRInput.Button.SecondaryHandTrigger));
        if (timedic.Count > 0)
        {
            List<string> remove = new List<string>() ;
            
            foreach (KeyValuePair<string, float> value in timedic)
            {
                if(timedic[value.Key]<Time.time)
                {
                    remove.Add(value.Key);
                }
            }
            for(int i = 0;i<remove.Count;i++)
            {
                timedic.Remove(remove[i]);
            }
        }
    }
}
