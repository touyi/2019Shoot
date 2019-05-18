using System.Collections.Generic;
using GamePlay.Actor;
using UnityEngine;

namespace GamePlay
{
    public class WaveInfo
    {

        public List<ActorBuildData> ProductWaveEnemyDatas(int nWave)
        {
            List<ActorBuildData> datas = new List<ActorBuildData>();
            int num = nWave;
            for (int i = 0; i < num; i++)
            {
                ActorBuildData data = ActorBuildData.Get();
                data.BornWorldPos = GenBornPos();
                data.type = ActorType.Enemy;
                data.HP = Mathf.Sqrt(nWave * 50);
                data.MaxHp = data.HP;
                data.Power = 10;
                datas.Add(data);
            }
            return datas;
        }
//        Z<= 160
//        10 <= Y <= 160
//        -265 <= X <= 310
        private Vector3 GenBornPos()
        {
            return new Vector3(Random.Range(-265,311),Random.Range(10,161),Random.Range(140,161));
        }
    }
}