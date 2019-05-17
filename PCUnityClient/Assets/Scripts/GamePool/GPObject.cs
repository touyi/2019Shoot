using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GPObject : MonoBehaviour
{
	public bool IsReturn { get; set; }
	public static string ResName()
	{ 
		return "";  
	}

    public abstract void Awake();

    public abstract void Start();

	public abstract void Destory();

}
