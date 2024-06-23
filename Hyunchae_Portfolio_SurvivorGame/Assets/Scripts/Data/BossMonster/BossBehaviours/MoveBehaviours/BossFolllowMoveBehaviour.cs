using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFolllowMoveBehaviour : BaseBossMoveBehaviour
{
    private const float FOLLOWTIME = 5.0f;

    private float curFollowTime = 0;

    public override void Update()
    {
        if (!isEndBehaviour)
        {
            Excute();
        }
    }

    public override void Excute()
    {

        pos = bossTransform.position;

        direction = (targetTransform.position - bossTransform.position).normalized;

        pos.x += direction.x * Time.deltaTime * moveSpeed;
        pos.y += direction.y * Time.deltaTime * moveSpeed;

        bossTransform.position = pos;

        curFollowTime += Time.deltaTime;

        if(curFollowTime >= FOLLOWTIME)
        {
            curFollowTime = 0;
            OnEndBehaviourAction?.Invoke();
        }
    }

}
