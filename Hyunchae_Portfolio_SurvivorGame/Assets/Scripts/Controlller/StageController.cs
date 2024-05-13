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
        //int count = 3;

        //for(int i = 0; i < count; i++)
        //{
        //    int monsterUid = i;

        //    MonsterModel model = monsterManager.GetMonsterModelToUid(monsterUid);

        //    MonsterController monster = monsterManager.GetMonster();

        //    monster.SetPlayerTransform(playerTransform);

        //    monster.SetMonsterModel(model);


        //}

        //MonsterModel model2 = monsterManager.GetMonsterModelToUid(0);

        //MonsterController monster2 = monsterManager.GetMonster();

        //monster2.SetPlayerTransform(playerTransform);

        //monster2.SetMonsterModel(model2);

        //model2 = monsterManager.GetMonsterModelToUid(0);

        //monster2 = monsterManager.GetMonster();

        //monster2.SetPlayerTransform(playerTransform);

        //monster2.SetMonsterModel(model2);

        //monster2 = monsterManager.GetMonster();

        //monster2.SetPlayerTransform(playerTransform);

        //monster2.SetMonsterModel(model2);


        float threshold = 0.5f; // 임계값 설정


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // x 값이 -10에 가까워질수록 false를 반환할 확률이 높아지도록 조절
            //bool xIsPositive = Random.Range(0f, 1f) > Mathf.Abs(playerTransform.position.x) / 10f + threshold;
            //bool yIsPositive = Random.Range(0f, 1f) > Mathf.Abs(playerTransform.position.y) / 10f + threshold;

            MonsterModel model2 = monsterManager.GetMonsterModelToUid(0);
            MonsterController monster2 = monsterManager.GetMonster();

            //float posX = 0;
            //float posY = 0;

            int radius = 5;

            //if (xIsPositive)
            //{
            //    posX = Random.Range(-9, playerTransform.position.x - radius + 1);

            //}
            //else
            //{
            //    posX = Random.Range(playerTransform.position.x + radius,9 + 1);
            //}

            //if(yIsPositive)
            //{
            //    posY = Random.Range(-9 ,playerTransform.position.y - radius + 1);
            //}
            //else
            //{
            //    posY = Random.Range(playerTransform.position.y + radius, 9 + 1);

            //}

            //float probabilityX = (playerTransform.position.x + 10f) / 20f; // 플레이어 x 포지션을 [0, 1] 범위로 정규화
            //float probabilityY = (playerTransform.position.y + 10f) / 20f; // 플레이어 y 포지션을 [0, 1] 범위로 정규화
              
            //bool posXIsPositive = Random.Range(0f, 1f) < threshold;
            //bool posYIsPositive = Random.Range(0f, 1f) < threshold;

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

            //float distanceX = Random.Range(minDistance, maxDistance);
            //float distanceY = Random.Range(minDistance, maxDistance);

            // 원 바깥에 위치한 지점을 계산합니다.
            //Vector3 spawnPosition = centerPoint.position + direction * distance;


            //float monsterPosX = posXIsPositive ?  Random.Range(playerTransform.position.x + radius, 10f) : Random.Range(-10f, playerTransform.position.x - radius);
            //float monsterPosY = posYIsPositive ? Random.Range(playerTransform.position.y + radius, 10f) : Random.Range(-10f, playerTransform.position.y - radius);

            //Vector2 pos = new Vector2(monsterPosX, monsterPosY);

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
