using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 雷达项目注册代理 防止雷达Awake 和 代理的物体Awake 不同步导致注册失败的情况
 */
public class RaderAgent  {

    static RaderAgent _instance = null;
    public delegate void VoidDelgate(Itemagent it);
    event VoidDelgate DRegist;
    event VoidDelgate DUnRegest;
    public static RaderAgent Instance
    {
        get
        {
            if (_instance == null)
                _instance = new RaderAgent();
            return _instance;
        }
    }
    
    HashSet<Itemagent> _items = new HashSet<Itemagent>();
    public void AddRegistAction(VoidDelgate func)
    {
        if (func == null) return;
        DRegist += func;
        foreach(Itemagent it in _items)
        {
            func(it);
        }
    }
    public void AddUnRegistAction(VoidDelgate func)
    {
        if (func == null) return;
        DUnRegest += func;
    }
    public void Regist(Itemagent it)
    {
        if(!_items.Contains(it))
        {
            _items.Add(it);
        }
        if(DRegist!=null)
            DRegist(it);
    }

    public void UnRegist(Itemagent it)
    {
        if(_items.Contains(it))
        {
            _items.Remove(it);
        }
        if(DUnRegest!=null)
            DUnRegest(it);
    }
    
}
