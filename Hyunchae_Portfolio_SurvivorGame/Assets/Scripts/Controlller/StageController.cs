using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController
{
    private MonsterManager monsterManager;
    private Transform playerTransform;
    private SpawnPointCalculator spawnPointCalculater;
    private StageManager stageManager;

    private int curStage = 0;
    private int curWave = 0;

    public void Init(Transform _playerTransform)
    {
        stageManager = StageManager.getInstance;
        monsterManager = MonsterManager.getInstance;
        spawnPointCalculater = new SpawnPointCalculator();
        SetPlayerTransform(_playerTransform);

        TestInputKey();

    }

    public void SetStageIndex(int _stageIdx)
    {
        curStage = _stageIdx;
        stageManager.SetCurStage(curStage);
    }

    public void SetMapData(MapData _mapData)
    {
        spawnPointCalculater.SetMapData(_mapData);
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

    }

    public void Update()
    {
        TestInputKey();
    }

    public void TestSpawnMonster()
    {

        MonsterModel model = monsterManager.GetMonsterModelToUid(0);
        MonsterController monster = monsterManager.GetMonster();

        if (spawnPointCalculater.GetSpawnPosition(playerTransform.position, out Vector2 monPos))
        {
            Debug.Log(monPos);

            monster.GetMonsterTransform.position = monPos;
            monster.SetPlayerTransform(playerTransform);
            monster.SetMonsterModel(model);
        }
        else
        {
            TestSpawnMonster();
        }
        


    }
}
