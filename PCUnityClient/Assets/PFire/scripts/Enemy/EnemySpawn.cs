using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class EnemySpawn : MonoBehaviour {


    Transform planeSpawnPos;        // 飞机产生位置
    StreamReader sreader;
    GameObject planePrefabs;
    int initPlaneNum;

    List<GameObject> planeList = new List<GameObject>();

    int enemyPlaneCount;
    bool isNewPlane;
    bool isEnd;
    private void Start()
    {
        isNewPlane = false;
        isEnd = true;
        planePrefabs = FGameObjectBar._instance.planePrefabs;
        planeSpawnPos = FGameObjectBar._instance.planeSpawnPos.transform;
        sreader = null;
    }
    private void Update()
    {
        if(isWaveEnd() && !isEnd)
        {
            creatNewWave();
            isNewPlane = false;
        }
    }
    public void SetPlaneText(string path)
    {
        if(sreader!=null)
        {
            sreader.Close();
        }
        BeginPlaneSpawn();
        sreader = new StreamReader(Application.streamingAssetsPath + path, Encoding.Default);
    }
    public void SetNumIncrease(int _num)
    {
        initPlaneNum = _num;
        BeginPlaneSpawn();
    }
    void BeginPlaneSpawn()
    {
        clearPlane();
        isNewPlane = true;
        isEnd = false;
    }
    public void clearPlane()
    {
        // 摧毁所有的地方飞机
        foreach (GameObject it in planeList)
        {
            if (it == null)
                continue;
            Destroy(it);
        }
        planeList.Clear();
        isEnd = true;
    }
    public bool isWaveEnd()
    {
        return isNewPlane;
    }
    public bool isAllWaveEnd()
    {
        return isEnd;
    }
    void creatNewWave()
    {
        if (sreader != null)
        {
            string num = sreader.ReadLine();
            if (num == null)
            {
                isEnd = true;
                return;
            }
            StartCoroutine(DelaySpawnPlane(5, int.Parse(num)));
        }
        else
        {
            StartCoroutine(DelaySpawnPlane(5, initPlaneNum++));
        }
    }
    public void PlaneDestory(GameObject go)
    { 
        planeList.Remove(go);
        enemyPlaneCount -= 1;
        if (enemyPlaneCount <= 0)
        {
            isNewPlane = true;
        }
    }
    IEnumerator DelaySpawnPlane(float timer, int num)
    {
        yield return new WaitForSeconds(timer);
        if (isEnd) yield break;
        int index = UnityEngine.Random.Range(0, planeSpawnPos.childCount);
        int cnt = 0;
        enemyPlaneCount = num;
        while (cnt < num)
        {
            GameObject go = GameObject.Instantiate(planePrefabs);
            Transform child = planeSpawnPos.GetChild(index % planeSpawnPos.childCount);
            go.transform.position = child.position;
            go.transform.rotation = child.rotation;
            go.GetComponent<PlaneDestoryMessage>().eSpawn = this;
            planeList.Add(go);
            index++;
            cnt++;
        }
    }
}
