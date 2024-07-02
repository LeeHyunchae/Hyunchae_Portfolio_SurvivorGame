using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class StageController
{
    private MonsterManager monsterManager;
    private GlobalData globalData;
    private PlayerController playerController;
    private SpawnPointCalculator spawnPointCalculater;
    private StageManager stageManager;
    private UIManager uiManager;
    private AugmentManager augmentManager;

    private int curStage = 0;
    private int curWave = -1;

    private int[] monsterGroupUidArr;
    private MonsterGroupData curMonsterGroupData;

    private float waveEndTime = 50;
    private float curWaveTime;

    private bool isWaveEnd = false;
    private IngamePanelController ingamePanel;

    private bool isBossWave = false;
    private MapData mapData;

    public void Init(PlayerController _player)
    {
        stageManager = StageManager.getInstance;
        monsterManager = MonsterManager.getInstance;
        augmentManager = AugmentManager.getInstance;
        uiManager = UIManager.getInstance;
        globalData = GlobalData.getInstance;
        SetPlayerController(_player);

        spawnPointCalculater = new SpawnPointCalculator();

        ShopPanelController shopPanel = uiManager.AddCachePanel<ShopPanelController>("UI/ShopPanel");
        shopPanel.OnClickNextWaveAction = StartWave;

        AugmentPanelController augmentPanel = uiManager.AddCachePanel<AugmentPanelController>("UI/AugmentPanel");
        augmentPanel.OnHideAugmentPanelAction = OnHideAugmentPanel;

        augmentManager.onRefreshAgumentActionDict[(int)EAugmentType.MONSTERSPAWN] = OnRefreshAugment;
    }

    public void SetStageIndex(int _stageIdx)
    {
        curStage = _stageIdx;

        StageData stageData = stageManager.GetStageData(curStage);

        monsterGroupUidArr = stageData.WaveMonsterGroupID;

        StartWave();
    }

    public void SetMapData(MapData _mapData)
    {
        mapData = _mapData;
        spawnPointCalculater.SetMapData(mapData);
    }

    public void SetIngamePanel(IngamePanelController _ingamePanelController)
    {
        ingamePanel = _ingamePanelController;
    }

    private void SetPlayerController(PlayerController _player)
    {
        playerController = _player;
    }

    public void Update()
    {
        if (globalData.GetPause)
        {
            return;
        }

        if (isWaveEnd)
        {
            return;
        }

        CheckSpawnTime();
    }

    public void CheckSpawnTime()
    {
        curWaveTime += Time.deltaTime;

        if(curWaveTime >= waveEndTime)
        {
            EndWave();
            return;
        }

        if(isBossWave)
        {
            SpawnBossMonster();
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
        if (spawnPointCalculater.GetSpawnPosition(playerController.transform.position, out monPos))
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

            if (globalData.GetPause)
            {
                await UniTask.Yield();
            }
            else
            {
                SpawnMonster(spawnData);
                await UniTask.Delay((int)(spawnData.respawnCycleTime * 1000)); // respawnCycleTime as milliSec
            }


            //sw.Stop();
            //UnityEngine.Debug.Log($"WATCH :> {sw.ElapsedMilliseconds}ms");

        }

        UnityEngine.Debug.Log("Stop Spawn Monster ID : " + spawnData.monsterUID + " Count :" + spawnData.monsterCount);
    }

    private void EndWave()
    {
        isWaveEnd = true;
        curWaveTime = 0;

        ingamePanel.EndWave();

        int count = curMonsterGroupData.monsterSpawnDatas.Count;

        for (int i = 0; i < count; i++)
        {
            MonsterSpawnData spawnData = curMonsterGroupData.monsterSpawnDatas[i];

            spawnData.isSpawnStart = false;
        }

        monsterManager.ReleaseAllAliveMonster();

        if(curWave % 5 == 0)
        {
            uiManager.Show<AugmentPanelController>("UI/AugmentPanel");
        }
        else
        {
            uiManager.Show<ShopPanelController>("UI/ShopPanel");
        }
    }

    private void StartWave()
    {
        isWaveEnd = false;
        curWaveTime = 0;
        curWave++;
        SetPlayerPos();

        stageManager.SetCurWave(curWave + 1);

        //if(curWave == monsterGroupUidArr.Length -1)
        //{
        //    isBossWave = true;
        //}

        if (curWave == 2)
        {
            isBossWave = true;
        }

        if (curWave >= monsterGroupUidArr.Length)
        {
            //Todo GameResultPanel && SceneChange
            globalData.UnloadScene();
            uiManager.UnloadScene();
            SceneChanger.getInstance.ChangeScene("MainScene");
            return;
        }

        SetMonsterToCurWave();
        ingamePanel.StartWave(curWave,waveEndTime);
    }

    private void SetMonsterToCurWave()
    {
        curMonsterGroupData = stageManager.GetMonsterGroupData(monsterGroupUidArr[curWave]);
    }

    private void OnHideAugmentPanel()
    {
        uiManager.Show<ShopPanelController>("UI/ShopPanel");
    }

    private void OnRefreshAugment()
    {

    }

    private void SpawnBossMonster()
    {
        isBossWave = false;

        BossMonsterController boss = monsterManager.GetBossMonster();

        GetSpawnPosition(out Vector2 monPos);

        boss.GetTransform().position = monPos;
        boss.SpawnBoss();
    }

    private void SetPlayerPos()
    {
        Vector2 pos = Vector2.zero;

        pos.x = Random.Range(-mapData.mapWidth * 0.5f, mapData.mapWidth * 0.5f);
        pos.x = Random.Range(-mapData.mapHeight * 0.5f, mapData.mapHeight * 0.5f);

        playerController.StartWave(pos);
    }
}
