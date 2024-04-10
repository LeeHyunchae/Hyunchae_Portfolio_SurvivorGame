using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageData
{
    public int stage;
    public string stageName;
    public float stageTime;
    public bool isRandomPos;

    public List<EnemyData> enemies = new List<EnemyData>();
}

[Serializable]
public class EnemyData
{
    public string name;
    public float spawnCycleTime;
    public float spawnStartTime;
}

public class SpawnSetting : MonoBehaviour
{
    public List<StageData> stageDatas = new List<StageData>();


    public void DebugLogStageData(int _stageNum)
    {
        if(_stageNum >= stageDatas.Count)
        {
            return;
        }

        Debug.Log("Stage : " + stageDatas[_stageNum].stage);
        Debug.Log("StageName : " + stageDatas[_stageNum].stageName);
        Debug.Log("StageTime : " + stageDatas[_stageNum].stageTime);
        Debug.Log("StageIsRandomSpawn : " + stageDatas[_stageNum].isRandomPos);

        for(int i = 0; i< stageDatas[_stageNum].enemies.Count;i++)
        {
            EnemyData e = stageDatas[_stageNum].enemies[i];

            Debug.Log("EnemyName : " + e.name);
            Debug.Log("EnemySpawnCycle : " + e.spawnCycleTime);
            Debug.Log("EnemySpawnStartTime : " + e.spawnStartTime);
        }
    }
}
