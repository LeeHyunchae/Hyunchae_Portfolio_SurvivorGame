using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMoveBehaviour : MonsterBehaviour
{
    private Vector2 pos;

    public override MonsterBehaviour DeepCopy()
    {
        return new FollowMoveBehaviour();
    }

    public override void Update()
    {
        base.Update();

        pos = monsterTransform.position;

        Vector2 direction = (targetTransform.position - monsterTransform.position).normalized;

        pos.x += direction.x * Time.deltaTime * 2;
        pos.y += direction.y * Time.deltaTime * 2;

        monsterTransform.position = pos;
    }
}
