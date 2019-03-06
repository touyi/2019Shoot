using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotaSelf : MonoBehaviour {
    RectTransform recttrans;
    public float speed;
    void Awake()
    {
        recttrans = GetComponent<RectTransform>();
    }
	// Update is called once per frame
	void Update () {
        recttrans.Rotate(new Vector3(0f, 0f, speed * Time.deltaTime));
	}
}
