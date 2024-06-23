using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSequenceCircleShoot : BaseBossAttackBehaviour
{
    private const int SHOOTCOUNT = 20;
    private const float CIRClEDEGREE = 360f;

    private const float RELOADTIME = 0.2f;

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
        float divideDegree = CIRClEDEGREE / SHOOTCOUNT;

        direction = (targetTransform.position - bossTransform.position).normalized;

        float firstAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float angle;

        OnStartBehaviourAction?.Invoke();

        if(isShoot)
        {
            curReloadTime += Time.deltaTime;
            if(curReloadTime >= RELOADTIME)
            {
                curReloadTime = 0;
                isShoot = false;
            }
        }
        else
        {
            angle = firstAngle + (divideDegree * curShootCount);

            Vector2 projectileDirection;

            projectileDirection.x = Mathf.Cos(angle * Mathf.Deg2Rad);
            projectileDirection.y = Mathf.Sin(angle * Mathf.Deg2Rad);

            damageData.direction = projectileDirection;

            Projectile projectile = itemManager.GetProjectile();

            projectile.SetSprite("Tier2_Props_12");
            projectile.SetTarget(target);
            projectile.SetPrjectileInfo(damageData, bossTransform.position, attackRange);

            curShootCount++;
            isShoot = true;

            if(curShootCount >= SHOOTCOUNT)
            {
                curShootCount = 0;
                curReloadTime = 0;
                isEndBehaviour = true;
                OnEndBehaviourAction?.Invoke();
            }
            
        }
        
    }

}
