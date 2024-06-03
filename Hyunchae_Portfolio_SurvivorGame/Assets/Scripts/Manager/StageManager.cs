using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    private Dictionary<int, StageData> stageDataDict = new Dictionary<int, StageData>();
    private Dictionary<int, MonsterGroupData> monsterGroupDataDict = new Dictionary<int, MonsterGroupData>();

    public override bool Initialize()
    {
        LoadData();
        return base.Initialize();
    }

    private void LoadData()
    {
        List<StageData> stageDatas = TableLoader.LoadFromFile<List<StageData>>("Stage/TestStage");

        int count = stageDatas.Count;

        for (int i = 0; i < count; i++)
        {
            StageData stageData = stageDatas[i];

            stageDataDict.Add(stageData.StageID, stageData);
        }

        List<JsonMonsterGroupData> jsonMonsterGroupDatas = TableLoader.LoadFromFile<List<JsonMonsterGroupData>>("Stage/TestMonsterGroup");

        count = jsonMonsterGroupDatas.Count;

        for(int i = 0; i < count; i++)
        {
            JsonMonsterGroupData jsonData = jsonMonsterGroupDatas[i];

            MonsterGroupData monsterGroupData = new MonsterGroupData();
            monsterGroupData.monsterGroupUID = jsonData.MonsterGroupID;
            monsterGroupData.monsterSpawnDatas = new List<MonsterSpawnData>();

            MonsterSpawnData spawnData = new MonsterSpawnData();
            spawnData.monsterUID = jsonData.MonsterID;
            spawnData.monsterCount = jsonData.MonsterNumber;
            spawnData.spawnStartTime = jsonData.FirstSpawnTime;
            spawnData.spawnEndTime = jsonData.EndSpawnTime;
            spawnData.respawnCycleTime = jsonData.RespawnCycle;

            if(monsterGroupDataDict.ContainsKey(monsterGroupData.monsterGroupUID))
            {
                monsterGroupDataDict[monsterGroupData.monsterGroupUID].monsterSpawnDatas.Add(spawnData);
            }
            else
            {
                monsterGroupData.monsterSpawnDatas.Add(spawnData);
                monsterGroupDataDict.Add(monsterGroupData.monsterGroupUID, monsterGroupData);
            }
        }
    }

    public StageData GetStageData(int _stageUID)
    {
        stageDataDict.TryGetValue(_stageUID, out StageData data);

#if UNITY_EDITOR

        if (data == null)
        {
            Debug.Log("Not Exist StageData");
        }
#endif

        return data;

    }

    public MonsterGroupData GetMonsterGroupData(int _monsterGroupUID)
    {
        monsterGroupDataDict.TryGetValue(_monsterGroupUID, out MonsterGroupData data);

#if UNITY_EDITOR

        if (data == null)
        {
            Debug.Log("Not Exist MonsterGroupData");
        }
#endif

        return data;
    }
}
