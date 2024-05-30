using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    private Dictionary<int, StageData> stageDataDict = new Dictionary<int, StageData>();
    private Dictionary<int, WaveGroupData> waveGroupDataDict = new Dictionary<int, WaveGroupData>();
    private Dictionary<int, MonsterGroupData> monsterGroupDataDict = new Dictionary<int, MonsterGroupData>();

    private int curStage = 0;

    public void SetCurStage(int _selectStageIdx) => curStage = _selectStageIdx;

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

            stageDataDict.Add(stageData.stageUID, stageData);
        }

        List<WaveGroupData> waveGroupDatas = TableLoader.LoadFromFile<List<WaveGroupData>>("Stage/TestWaveGroup");

        count = waveGroupDatas.Count;

        for (int i = 0; i < count; i++)
        {
            WaveGroupData waveGroupData = waveGroupDatas[i];

            waveGroupDataDict.Add(waveGroupData.waveGruopUID, waveGroupData);
        }

        List<MonsterGroupData> monsterGroupDatas = TableLoader.LoadFromFile<List<MonsterGroupData>>("Stage/TestWaves");

        count = monsterGroupDatas.Count;

        for(int i = 0; i < count; i++)
        {
            MonsterGroupData monsterGroupData = monsterGroupDatas[i];

            monsterGroupDataDict.Add(monsterGroupData.monsterGroupUID, monsterGroupData);
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

    public WaveGroupData GetWaveGroupData(int _waveGroupUID)
    {
        waveGroupDataDict.TryGetValue(_waveGroupUID, out WaveGroupData data);

#if UNITY_EDITOR

        if (data == null)
        {
            Debug.Log("Not Exist WaveGroupData");
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
