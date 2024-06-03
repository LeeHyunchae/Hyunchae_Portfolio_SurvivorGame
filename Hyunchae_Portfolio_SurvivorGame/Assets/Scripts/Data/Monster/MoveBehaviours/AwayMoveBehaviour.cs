using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwayMoveBehaviour : MonsterBehaviour
{
    private float moveSpeed;
    private float attackRange;
    private float awayRange;

    public override MonsterBehaviour DeepCopy()
    {
        return new AwayMoveBehaviour();
    }

    public override void SetMonsterModel(MonsterModel _model)
    {
        base.SetMonsterModel(_model);

        moveSpeed = model.status.moveSpeed;
        awayRange = model.status.attackRange * 0.5f;
        attackRange = model.status.attackRange;
    }

    public override void Update()
    {
        Excute();
    }

    protected override void Excute()
    {
        float distance = Vector2.Distance(targetTransform.position, monsterTransform.position);
        Vector2 direction = (targetTransform.position - monsterTransform.position).normalized;

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
