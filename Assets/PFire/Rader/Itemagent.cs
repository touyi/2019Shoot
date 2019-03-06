using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemagent : MonoBehaviour {
    public Color color;
    public Sprite img;
	void Awake()
    {
        RaderAgent.Instance.Regist(this);
    }
    void OnEnable()
    {
        RaderAgent.Instance.Regist(this);
    }
    void OnDisable()
    {
        RaderAgent.Instance.UnRegist(this);
    }
    void OnDestory()
    {
        RaderAgent.Instance.UnRegist(this);
    }
}
