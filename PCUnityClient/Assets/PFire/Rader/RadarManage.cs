using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * 雷达控制
 */
public class RadarManage : MonoBehaviour {

    public GameObject itemPrefabs;
    public Transform _center;
    public float maxLength = 100;
    private RectTransform RadarTrans;
    private float mapsize;
    public bool isRotation = true;
    Dictionary<Itemagent, RectTransform> _items = new Dictionary<Itemagent, RectTransform>();
    void Awake()
    {
        RaderAgent.Instance.AddRegistAction(Regist);
        RaderAgent.Instance.AddUnRegistAction(UnRegist);
        RadarTrans = GetComponent<RectTransform>();
        mapsize = RadarTrans.sizeDelta.x / 2f;
    }
    public void UnRegist(Itemagent it)
    {
        if(_items.ContainsKey(it))
        {
            if(_items[it]!=null)
                Destroy(_items[it].gameObject);
            _items.Remove(it);
        }
    }
    public void Regist(Itemagent it)
    {
        if(!_items.ContainsKey(it))
        {
            GameObject go = Instantiate(itemPrefabs,transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = new Vector3(1, 1, 1);
            Image img = go.GetComponent<Image>();
            img.color = it.color;
            img.sprite = it.img;
            RectTransform rect = go.GetComponent<RectTransform>();
            _items.Add(it, rect);
        }
    }
    public void SetCenter(Transform center)
    {
        _center = center;
    }
    void Update()
    {
        if (_center == null) return;
        float cosx = Mathf.Cos(_center.transform.rotation.eulerAngles.y * Mathf.Deg2Rad);
        float sinx = Mathf.Sin(_center.transform.rotation.eulerAngles.y * Mathf.Deg2Rad);
        foreach(KeyValuePair<Itemagent,RectTransform> it in _items)
        {
            Vector2 vec2d = new Vector2(it.Key.transform.position.x-_center.position.x, it.Key.transform.position.z-_center.position.z);
            if (vec2d.magnitude > maxLength)
            {
                it.Value.gameObject.SetActive(false);
            }
            else
            {
                it.Value.gameObject.SetActive(true);
                Vector2 dir = vec2d / maxLength * mapsize;
                if(isRotation)
                    dir = new Vector2(dir.x * cosx - dir.y * sinx, dir.x * sinx+dir.y * cosx );
                it.Value.localPosition = new Vector3(dir.x, dir.y, 0f);
            }
            
        }
    }
}
