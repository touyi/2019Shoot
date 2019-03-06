using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PShine : MonoBehaviour {

    Image img;
    public float max = 10f;
    public float min = 10f;
    public float speed = 0.5f;
    int reverse = 1;
    void Awake()
    {
        img = GetComponent<Image>();
    }
    void Update()
    {
        float num = img.color.a*255f + speed * Time.deltaTime * reverse;
        if(num>max)
        {
            reverse = -reverse;
            num = max;
        }
        if(num<min)
        {
            reverse = -reverse;
            num = min;
        }
        
        img.color = new Color(img.color.r, img.color.g, img.color.b, num/255f);

    }
}
