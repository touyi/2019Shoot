using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEnergyCount : MonoBehaviour {

    public delegate void LineEnergyChange(float f);
    public event LineEnergyChange OnLineEnergyChange;
    public bulelineControl bcontrol;
    public SliderControl slider;
    void Update()
    {
        slider.Value = bcontrol.LineEnergy / bcontrol.MaxEnergy;

        // 2018-01-17 新增 能量改变委托
        if (OnLineEnergyChange!=null)
            OnLineEnergyChange(bcontrol.LineEnergy / bcontrol.MaxEnergy); 
    }
}
