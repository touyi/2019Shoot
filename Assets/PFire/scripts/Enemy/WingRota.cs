using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingRota : MonoBehaviour {

    public float speed = 20;
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, speed) * Time.deltaTime);
	}
}
