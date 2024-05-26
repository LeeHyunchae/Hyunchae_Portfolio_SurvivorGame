using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController
{
    private MonsterManager monsterManager;
    private Transform playerTransform;
    private SpawnPointCalculator spawnPointCalculater;

    public void Init(Transform _playerTransform)
    {
        monsterManager = MonsterManager.getInstance;
        spawnPointCalculater = new SpawnPointCalculator();
        SetPlayerTransform(_playerTransform);

        TestInputKey();

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
