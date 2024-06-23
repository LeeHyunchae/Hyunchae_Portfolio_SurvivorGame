using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTripleShootBehaviour : BaseBossAttackBehaviour
{
    private const int SHOOTCOUNT = 3;
    private const float RELOADTIME = 0.5f;
    private const int ANGLE = 45;

    private bool isShoot = false;
    private float curReloadTime = 0;
    private int curShootCount = 0;

    public override void Update()
    {
        if (!isEndBehaviour)
        {
            Excute();
        }
    }

    public override void Excute()
    {
        direction = (targetTransform.position - bossTransform.position).normalized;

        float firstAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float angle = firstAngle - ANGLE;

        OnStartBehaviourAction?.Invoke();

        if (isShoot)
        {
            curReloadTime += Time.deltaTime;

            if (curReloadTime >= RELOADTIME)
            {
                curReloadTime = 0;
                isShoot = false;
            }
        }
        else
        {
            for (int i = 0; i < SHOOTCOUNT; i++)
            {
                
                Vector2 projectileDirection;

                projectileDirection.x = Mathf.Cos(angle * Mathf.Deg2Rad);
                projectileDirection.y = Mathf.Sin(angle * Mathf.Deg2Rad);

                damageData.direction = projectileDirection;

                Projectile projectile = itemManager.GetProjectile();

                if(angle == firstAngle)
                {
                    projectile.SetSprite("Tier2_Props_12");
                }

                projectile.SetTarget(target);
                projectile.SetPrjectileInfo(damageData, bossTransform.position, attackRange);

                angle += ANGLE;
            }

            curShootCount++;
            isShoot = true;
        }

        if(curShootCount == SHOOTCOUNT)
        {
            curReloadTime = 0;
            isShoot = false;
            curShootCount = 0;
            isEndBehaviour = true;
            OnEndBehaviourAction?.Invoke();
        }

    }

}
