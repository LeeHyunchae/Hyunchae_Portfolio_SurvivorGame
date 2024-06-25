using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDashBehaviour : BaseBossMoveBehaviour
{
    private const float READYTIME = 1.0f;

    private bool isStartDash = false;
    private bool isReadyDash = false;
    private float curReadyTime = 0;
    private Vector2 targetPos;

    public override void Update()
    {
        if(!isEndBehaviour)
        {
            Excute();
        }
    }

    public override void Excute()
    {
        if (!isStartDash)
        {
            distance = Vector2.Distance(targetTransform.position, bossTransform.position);

            if(!isReadyDash)
            {
                if(distance < attackRange)
                {
                    isReadyDash = true;
                }

                pos = bossTransform.position;

                direction = (targetTransform.position - bossTransform.position).normalized;

                pos.x += direction.x * Time.deltaTime * moveSpeed;
                pos.y += direction.y * Time.deltaTime * moveSpeed;

                bossTransform.position = pos;
            }
            else
            {
                //Todo Sprite Color Swap

                direction = (targetTransform.position - bossTransform.position).normalized;

                curReadyTime += Time.deltaTime;

                if(curReadyTime > READYTIME)
                {
                    isStartDash = true;
                    dashStartPos = pos;
                    targetPos = targetTransform.position;
                }
            }

        }
        else
        {
            pos = bossTransform.position;

            pos.x += direction.x * Time.deltaTime * moveSpeed * 3;
            pos.y += direction.y * Time.deltaTime * moveSpeed * 3;

            bossTransform.position = pos;

            float dashDistanceToTarget = Vector2.Distance(targetPos, pos);

            if(dashDistanceToTarget <= 0.25f)
            {
                isStartDash = false;
                isEndBehaviour = false;
                isReadyDash = false;
                curReadyTime = 0;
                OnEndBehaviourAction?.Invoke();
            }

            float dashDistanceToStartPos = Vector2.Distance(dashStartPos, pos);

            if (dashDistanceToStartPos >= attackRange)
            {
                isStartDash = false;
                isEndBehaviour = false;
                isReadyDash = false;
                curReadyTime = 0;
                OnEndBehaviourAction?.Invoke();
            }
        }
    }

}
