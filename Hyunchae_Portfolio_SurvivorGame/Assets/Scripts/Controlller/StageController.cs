using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Diagnostics;

public class StageController
{
    private MonsterManager monsterManager;
    private Transform playerTransform;
    private SpawnPointCalculator spawnPointCalculater;
    private StageManager stageManager;
    private UIManager uiManager;

    private int curStage = 0;
    private int curWave = -1;

    private int[] monsterGroupUIDarr;
    private MonsterGroupData curMonsterGroupData;

    private float waveEndTime = 50;
    private float curWaveTime;

    private bool isWaveEnd = false;
    private IngamePanelController ingamePanel;

    public void Init(Transform _playerTransform)
    {
        stageManager = StageManager.getInstance;
        monsterManager = MonsterManager.getInstance;
        uiManager = UIManager.getInstance;
        spawnPointCalculater = new SpawnPointCalculator();
        SetPlayerTransform(_playerTransform);

        ShopPanelController shopPanel = uiManager.AddCachePanel<ShopPanelController>("UI/ShopPanel");
        shopPanel.OnClickNextWaveAction = StartWave;
    }

    public void SetStageIndex(int _stageIdx)
    {
        curStage = _stageIdx;

        StageData stageData = stageManager.GetStageData(curStage);

        monsterGroupUIDarr = stageData.WaveMonsterGroupID;

        StartWave();
    }

    public void SetMapData(MapData _mapData)
    {
        spawnPointCalculater.SetMapData(_mapData);
    }

    public void SetIngamePanel(IngamePanelController _ingamePanelController)
    {
        ingamePanel = _ingamePanelController;
    }

    private void SetPlayerTransform(Transform _transform)
    {
        playerTransform = _transform;
    }

    private void TestInputKey()
    {
       
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestSpawnMonster();
        }


        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EndWave();
        }
    }

    public void Update()
    {
        TestInputKey();

        if (isWaveEnd)
        {
            return;
        }

        CheckSpawnTime();
    }

    public void TestSpawnMonster()
    {

        MonsterModel model = monsterManager.GetMonsterModelToUid(2);
        MonsterController monster = monsterManager.GetMonster();

        if (spawnPointCalculater.GetSpawnPosition(playerTransform.position, out Vector2 monPos))
        {
            monster.GetMonsterTransform.position = monPos;
            monster.SetMonsterModel(model);
        }
        else
        {
            TestSpawnMonster();
        }
    }

    public void CheckSpawnTime()
    {
        curWaveTime += Time.deltaTime;

        if(curWaveTime >= waveEndTime)
        {
            EndWave();
            return;
        }

        int count = curMonsterGroupData.monsterSpawnDatas.Count;

        for(int i = 0; i < count; i ++)
        {
            MonsterSpawnData spawnData = curMonsterGroupData.monsterSpawnDatas[i];

            if(spawnData.spawnEndTime < curWaveTime || spawnData.isSpawnStart)
            {
                continue;
            }

            if(spawnData.spawnStartTime <= curWaveTime)
            {
                spawnData.isSpawnStart = true;
                SpawnMonsterPeriodically(spawnData).Forget();
            }
        }
    }

    public void SpawnMonster(MonsterSpawnData _spawnData)
    {
        MonsterModel model = monsterManager.GetMonsterModelToUid(_spawnData.monsterUID);

        int count = _spawnData.monsterCount;

        for(int i = 0; i < count; i++)
        {
            GetSpawnPosition(out Vector2 monPos);

            MonsterController monster = monsterManager.GetMonster();
            monster.GetMonsterTransform.position = monPos;
            monster.SetMonsterModel(model);
        }
    }

    public void GetSpawnPosition(out Vector2 monPos)
    {
        if (spawnPointCalculater.GetSpawnPosition(playerTransform.position, out monPos))
        {
            return;
        }
        else
        {
            GetSpawnPosition(out monPos);
        }
    }

    private async UniTaskVoid SpawnMonsterPeriodically(MonsterSpawnData spawnData)
    {
        Stopwatch sw = new Stopwatch();
        

        while (curWaveTime < spawnData.spawnEndTime && !isWaveEnd)
        {
            //sw.Start();

            SpawnMonster(spawnData);
            await UniTask.Delay((int)(spawnData.respawnCycleTime * 1000)); // respawnCycleTime as milliSec

            //sw.Stop();
            //UnityEngine.Debug.Log($"WATCH :> {sw.ElapsedMilliseconds}ms");


        }

        UnityEngine.Debug.Log("Stop Spawn Monster ID : " + spawnData.monsterUID + " Count :" + spawnData.monsterCount);
    }

    private void EndWave()
    {
        isWaveEnd = true;
        curWaveTime = 0;

        int count = curMonsterGroupData.monsterSpawnDatas.Count;

        for (int i = 0; i < count; i++)
        {
            MonsterSpawnData spawnData = curMonsterGroupData.monsterSpawnDatas[i];

            spawnData.isSpawnStart = false;
        }

        uiManager.Show<ShopPanelController>("UI/ShopPanel");
        monsterManager.ReleaseAllAliveMonster();
    }

    private void StartWave()
    {
        isWaveEnd = false;
        curWaveTime = 0;
        curWave++;

        if(curWave >= monsterGroupUIDarr.Length)
        {
            curWave = monsterGroupUIDarr.Length - 1;
        }

        SetMonsterToCurWave();
        ingamePanel.StartWave(curWave,waveEndTime);
    }

    private void SetMonsterToCurWave()
    {
        curMonsterGroupData = stageManager.GetMonsterGroupData(monsterGroupUIDarr[curWave]);
    }
}
