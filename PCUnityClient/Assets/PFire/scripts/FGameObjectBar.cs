using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 场景物体面板类
 * 存放所有其他类需要的场景物体
 */
public class FGameObjectBar : MonoBehaviour {

    public static FGameObjectBar _instance = null;
    void OnEnable()
    {
        if(_instance ==null)
        _instance = this;
    }
    void Awake()
    {
        if(_instance==null)
        _instance = this;
    }
    [SerializeField, HeaderAttribute("Enemy")]
    public EnemySpawn enemySpawn;
    public GameObject planeSpawnPos;
    public GameObject planePrefabs;

    [SerializeField, HeaderAttribute("Player")]
    public GameObject myBase;

    [SerializeField, HeaderAttribute("Screen")]
    public GameObject InfoScreen;
    public GameObject PCScreen;
    
    [SerializeField,HeaderAttribute("Gun")]
    public GameObject lineGun;
    //public GameObject fireGun;
    public GameObject faintGun;
    public GameObject gunBase;

    [SerializeField, HeaderAttribute("UI")]
    public EncodedControl encodedControl;

    [SerializeField, HeaderAttribute("Other")]
    public FShakeCamera shakeCamera;
}
