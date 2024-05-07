using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMoveBehaviour : MonsterBehaviour
{
    private float moveSpeed;

    public override MonsterBehaviour DeepCopy()
    {
        return new FollowMoveBehaviour();
    }

    public override void SetMonsterModel(MonsterModel _model)
    {
        moveSpeed = _model.status.moveSpeed;
    }

    public override void Update()
    {
        Excute();
    }

    protected override void Excute()
    {
        pos = monsterTransform.position;

        Vector2 direction = (targetTransform.position - monsterTransform.position).normalized;

        pos.x += direction.x * Time.deltaTime * moveSpeed;
        pos.y += direction.y * Time.deltaTime * moveSpeed;

        monsterTransform.position = pos;
    }
}
