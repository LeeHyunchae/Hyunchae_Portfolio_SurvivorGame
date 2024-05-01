using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController
{
    private MonsterManager monsterManager;
    private Transform playerTransform;


    public void Init(Transform _playerTransform)
    {
        monsterManager = MonsterManager.getInstance;
        SetPlayerTransform(_playerTransform);

        TestMonster();

    }

    private void SetPlayerTransform(Transform _transform)
    {
        playerTransform = _transform;
    }

    private void TestMonster()
    {
        int count = 3;

        for(int i = 0; i < count; i++)
        {
            int monsterUid = i;

            MonsterModel model = monsterManager.GetMonsterModelToUid(monsterUid);

            MonsterController monster = monsterManager.GetMonster();

            monster.SetPlayerTransform(playerTransform);

            monster.SetMonsterModel(model);


        }
    }

    public void Update()
    {

    }
}
