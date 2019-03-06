using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTest : MonoBehaviour {

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClientKeyInfo.CChange = true;
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            ClientKeyInfo.Firecd = 0.1f;
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            ClientKeyInfo.SSure = true;
        }
    }
}
