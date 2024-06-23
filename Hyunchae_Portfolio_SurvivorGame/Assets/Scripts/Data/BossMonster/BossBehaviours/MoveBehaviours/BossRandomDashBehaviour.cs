using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRandomDashBehaviour : BaseBossMoveBehaviour
{
    private const float READYTIME = 1.0f;

    private bool isStartDash = false;
    private float curReadyTime = 0;

    public override void Update()
    {
        if (!isEndBehaviour)
        {
            Excute();
        }
    }

    public override void Excute()
    {
        if (!isStartDash)
        {
            distance = Vector2.Distance(targetTransform.position, bossTransform.position);

            if (distance > attackRange)
            {
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

                if (curReadyTime > READYTIME)
                {
                    isStartDash = true;
                    dashStartPos = pos;
                }
            }

        }
        else
        {
            pos = bossTransform.position;

            pos.x += direction.x * Time.deltaTime * moveSpeed * 2;
            pos.y += direction.y * Time.deltaTime * moveSpeed * 2;

            bossTransform.position = pos;

            float dashDistance = Vector2.Distance(dashStartPos, pos);

            if (dashDistance >= attackRange)
            {
                isStartDash = false;
                isEndBehaviour = false;
                curReadyTime = 0;
                OnEndBehaviourAction?.Invoke();
            }
        }
    }

}
