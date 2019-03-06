using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * 跟随显示屏上血条
 */
public class HealthySlider : MonoBehaviour {

    Slider slider;
	void Awake()
    {
        FGameInfo.Instance.OnInfoChange += OnHealthyChange;
        slider = GetComponent<Slider>();
    }
	
	void OnHealthyChange(FGameInfo.InfoChangeType type)
    {
        if(type==FGameInfo.InfoChangeType.PlayerHealthy)
        {
            slider.value = FGameInfo.Instance.PlayerHealthy / FGameInfo.Instance.maxPlayerHealthy;
        }
    }
    void OnDestory()
    {
        FGameInfo.Instance.OnInfoChange -= OnHealthyChange;
    }
}
