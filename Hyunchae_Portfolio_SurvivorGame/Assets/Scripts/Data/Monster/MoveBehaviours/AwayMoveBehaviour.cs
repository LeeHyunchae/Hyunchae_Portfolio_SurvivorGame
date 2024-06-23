using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwayMoveBehaviour : MonsterBehaviour
{
    public override MonsterBehaviour DeepCopy()
    {
        return new AwayMoveBehaviour();
    }

    public override void Update()
    {
        Excute();
    }

    protected override void Excute()
    {
        float distance = Vector2.Distance(targetTransform.position, monsterTransform.position);
        Vector2 direction = (targetTransform.position - monsterTransform.position).normalized;

        float attackRange = monsterStatus[(int)EMonsterStatus.MONSTER_ATTACKRANGE];
        float awayRange = attackRange * 0.5f;
        float moveSpeed = monsterStatus[(int)EMonsterStatus.MONSTER_MOVESPEED];


        if (distance < awayRange)
        {
            pos = monsterTransform.position;


            pos.x += -direction.x * Time.deltaTime * moveSpeed;
            pos.y += -direction.y * Time.deltaTime * moveSpeed;

            monsterTransform.position = pos;
        }
        else if(distance > attackRange)
        {
            pos = monsterTransform.position;

            pos.x += direction.x * Time.deltaTime * moveSpeed;
            pos.y += direction.y * Time.deltaTime * moveSpeed;

            monsterTransform.position = pos;
        }

    }
}
