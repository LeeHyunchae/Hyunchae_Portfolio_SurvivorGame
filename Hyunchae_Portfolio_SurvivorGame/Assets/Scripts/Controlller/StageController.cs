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
        //현재 웨이브 시간 체크
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
            // 몬스터 그룹 데이터에 저장된 스폰 시간을 초과하면 UniTask로 구현한 주기적 몬스터 스폰 함수 호출
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
        if(playerController == null)
        {
            monPos = Vector2.zero;
            return;
        }

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
        while (curWaveTime < spawnData.spawnEndTime && !isWaveEnd)
        {
            if(globalData.GetGameEnd)
            {
                spawnData.isSpawnStart = false;
                break;
            }
            else if (globalData.GetPause)
            {
                await UniTask.Yield();
            }
            else
            {
                SpawnMonster(spawnData);
                await UniTask.Delay((int)(spawnData.respawnCycleTime * 1000));
            }

        }

        spawnData.isSpawnStart = false;
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

        if (curWave == 2)
        {
            ResultPanelController resultPanel = uiManager.Show<ResultPanelController>("UI/ResultPanel");
            resultPanel.OnClearText();
            return;
        }

        if (curWave % 5 == 0)
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

        pos.x = Random.Range(-mapData.mapWidth * 0.5f + 1, mapData.mapWidth * 0.5f - 1);
        pos.x = Random.Range(-mapData.mapHeight * 0.5f + 1, mapData.mapHeight * 0.5f - 1);

        playerController.StartWave(pos);
    }
}
