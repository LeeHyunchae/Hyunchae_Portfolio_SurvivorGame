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
        /*
        int count = 3;

        for(int i = 0; i < count; i++)
        {
            int monsterUid = i;

            MonsterModel model = monsterManager.GetMonsterModelToUid(monsterUid);

            MonsterController monster = monsterManager.GetMonster();

            monster.SetPlayerTransform(playerTransform);

            monster.SetMonsterModel(model);


        }

        MonsterModel model2 = monsterManager.GetMonsterModelToUid(0);

        MonsterController monster2 = monsterManager.GetMonster();

        monster2.SetPlayerTransform(playerTransform);

        monster2.SetMonsterModel(model2);

        model2 = monsterManager.GetMonsterModelToUid(0);

        monster2 = monsterManager.GetMonster();

        monster2.SetPlayerTransform(playerTransform);

        monster2.SetMonsterModel(model2);

        monster2 = monsterManager.GetMonster();

        monster2.SetPlayerTransform(playerTransform);

        monster2.SetMonsterModel(model2);
        */


        float threshold = 0.5f; // 임계값 설정


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            MonsterModel model2 = monsterManager.GetMonsterModelToUid(0);
            MonsterController monster2 = monsterManager.GetMonster();

            int radius = 5;

            float angle = Random.Range(0f, 360f);

            float radians = angle * Mathf.Deg2Rad;

            Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

            Debug.Log(direction.x + "    " + direction.y);

            bool xIsPositive = direction.x > 0;
            bool yIsPositive = direction.y > 0;

            Vector2 distance = Vector2.zero;

            distance.x = xIsPositive ? Random.Range(playerTransform.position.x + radius, 10) : Random.Range(-10, playerTransform.position.x - radius + 1);
            distance.y = yIsPositive ? Random.Range(playerTransform.position.y + radius, 10) : Random.Range(-10, playerTransform.position.y - radius + 1);

            Debug.Log(distance.x + "    " + distance.y);

            Vector2 monPos = direction.normalized * distance;

            Debug.Log((distance.normalized * distance).x + "     " + (distance.normalized * distance).y);

            monster2.GetMonsterTransform.position = monPos;


            monster2.SetPlayerTransform(playerTransform);

            monster2.SetMonsterModel(model2);





        }

    }

    public void Update()
    {
        TestMonster();
    }
}
