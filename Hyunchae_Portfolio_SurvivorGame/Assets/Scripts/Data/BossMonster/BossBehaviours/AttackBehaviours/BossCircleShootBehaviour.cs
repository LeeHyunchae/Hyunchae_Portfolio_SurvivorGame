using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCircleShootBehaviour : BaseBossAttackBehaviour
{
    private const int SHOOTCOUNT = 20;
    private const float CIRClEDEGREE = 360f;

    public override void Update()
    {
        if (!isEndBehaviour)
        {
            Excute();
        }
    }

    public override void Excute()
    {
        float divideDegree = CIRClEDEGREE / SHOOTCOUNT;

        direction = (targetTransform.position - bossTransform.position).normalized;

        float firstAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float angle = firstAngle;

        OnStartBehaviourAction?.Invoke();

        for(int i = 0; i < SHOOTCOUNT; i++)
        {
            Vector2 projectileDirection;

            projectileDirection.x = Mathf.Cos(angle * Mathf.Deg2Rad);
            projectileDirection.y = Mathf.Sin(angle * Mathf.Deg2Rad);

            damageData.direction = projectileDirection;

            Projectile projectile = itemManager.GetProjectile();

            projectile.SetSprite("Tier2_Props_12");
            projectile.SetTarget(target);
            projectile.SetPrjectileInfo(damageData, bossTransform.position, attackRange);

            angle += divideDegree;
        }

        isEndBehaviour = true;
        OnEndBehaviourAction?.Invoke();
    }

}
