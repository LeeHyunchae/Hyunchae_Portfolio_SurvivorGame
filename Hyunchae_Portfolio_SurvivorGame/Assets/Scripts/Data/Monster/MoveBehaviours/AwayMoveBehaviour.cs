using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwayMoveBehaviour : MonsterBehaviour
{
    private float moveSpeed;
    private float awayRange;

    public override MonsterBehaviour DeepCopy()
    {
        return new AwayMoveBehaviour();
    }

    public override void SetMonsterModel(MonsterModel _model)
    {
        moveSpeed = _model.status.moveSpeed;
        awayRange = _model.status.attackRange * 0.5f;
    }

    public override void Update()
    {
        Excute();
    }

    protected override void Excute()
    {
        float distance = Vector2.Distance(targetTransform.position, monsterTransform.position);

        if (distance < awayRange)
        {
            pos = monsterTransform.position;

            Vector2 direction = (targetTransform.position - monsterTransform.position).normalized;

            pos.x += -direction.x * Time.deltaTime * moveSpeed;
            pos.y += -direction.y * Time.deltaTime * moveSpeed;

            monsterTransform.position = pos;
        }

    }
}
