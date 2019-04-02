using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PInput : MonoBehaviour {

    
    public static bool FireL()
    {
        // TODO
        if (false)
        {
            return true;
        }
        return false;
    }
    public static bool FireR()
    {
        // TODO
        if (false)
        {
            return true;
        }
        return false;
    }
    public static bool Sure()
    {
        if(Input.GetKeyDown(KeyCode.Space) 
            || ClientKeyInfo.SSure)
        {
            return true;
        }
        return false;
    }
    public static bool Fire()
    {
        if(Input.GetMouseButton(0) || ClientKeyInfo.fire)
        {
            return true;
        }
        return false;
    }
    public static Vector2 GetVec2dInput()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        return new Vector2(x, y);
    }
    public static bool GetChangeButton()
    {
        
        if(Input.GetKeyDown(KeyCode.Tab) || ClientKeyInfo.CChange)
        {
            return true;
        }
        return false;
    }
}
