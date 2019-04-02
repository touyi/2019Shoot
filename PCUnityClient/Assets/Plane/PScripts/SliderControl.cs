using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class SliderControl : MonoBehaviour {

    [SerializeField]
    [Range(0,1.0f)]
    float value = 0.5f;
    public Transform back;
    public Transform forward;
    public float Value
    {
        get
        {
            return value;
        }

        set
        {
            this.value = value;
            if (this.value > 1f)
                this.value = 1f;
            if (this.value < 0)
                this.value = 0f;
        }
    }
    void Update()
    {
        forward.localScale = new Vector3(back.localScale.x * value, back.localScale.y, back.localScale.z);
    }
}
