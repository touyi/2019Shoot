using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using assets;

public class GPGameObjectPool
{


	private static Dictionary<string,Queue<GameObject>> _pool = new Dictionary<string, Queue<GameObject>>();
	private static Dictionary<string,string> _resName = new Dictionary<string, string>();

	public static void PreLoad<T>(int count)where T : GPObject
	{
		string _type = typeof(T).ToString ();
		if(!_pool.ContainsKey(_type)){
			_pool.Add (_type, new Queue<GameObject> ());
		}
		Queue<GameObject> _itemPool = _pool [_type];
		GameObject _go = null;

		if(_itemPool.Count < count){
			string res = GetResName<T> (_type);
			// UnityEngine.Object obj = Resources.Load (res);
			UnityEngine.Object obj = AssetsManager.Instance.LoadPrefab(res);
			while (_itemPool.Count < count)
			{
				_go = GameObject.Instantiate (obj) as GameObject;
				var com = _go.GetComponent<T>();
				if(com == null) com = _go.AddComponent<T> ();
				com.IsReturn = true;
				_go.SetActive(false);
				_itemPool.Enqueue(_go);
			}
			
		}
	}
	//创建物体
	public static GameObject Creat<T>(Vector3 postion,Quaternion rotation)where T : GPObject
	{
		string _type = typeof(T).ToString ();
		if(!_pool.ContainsKey(_type)){
			_pool.Add (_type, new Queue<GameObject> ());
		}
		Queue<GameObject> _itemPool = _pool [_type];
		GameObject _go = null;
		T gpCom = null;

		if(_itemPool.Count == 0){
			string res = GetResName<T> (_type);
			// UnityEngine.Object obj = Resources.Load (res);
			UnityEngine.Object obj = AssetsManager.Instance.LoadPrefab(res);
			_go = GameObject.Instantiate (obj, postion, rotation) as GameObject;
			gpCom = _go.GetComponent<T>();
			if(gpCom == null) gpCom = _go.AddComponent<T> ();
		}else{
			_go = _itemPool.Dequeue ();
			_go.transform.position = postion;
			_go.transform.rotation = rotation;
			_go.SetActive (true);
			gpCom = _go.GetComponent<T> ();
			gpCom.Awake ();
			gpCom.Start ();
		}

		gpCom.IsReturn = false;
		return _go;
	}

	//放回物体
	public static void Return(GameObject _go)
	{
		GPObject com = _go.GetComponent<GPObject> ();
		if (com.IsReturn)
		{
			return;
		}

		com.IsReturn = true;
		if(com == null){
			Debug.LogError ("this gameobject isn't created by the pool");
			return;
		}
		string _type = com.GetType ().ToString ();
		if(!_pool.ContainsKey(_type)){
			_pool.Add (_type, new Queue<GameObject> ());
		}
		Queue<GameObject> _itemPool = _pool [_type];
		com = _go.GetComponent<GPObject> ();
		com.Destory ();
		_go.SetActive (false);
		_itemPool.Enqueue (_go);
	}
		
	static string GetResName<T>(string _type)where T : GPObject
	{
		if(_resName.ContainsKey(_type)){
			return _resName [_type];
		}
		MethodInfo info = typeof(T).GetMethod ("ResName", BindingFlags.Static | BindingFlags.Public);
		string res = info.Invoke (null, null).ToString();
		_resName.Add (_type, res);
		return res;
	}
}

