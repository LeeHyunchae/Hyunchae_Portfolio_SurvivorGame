using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSkillBehaviour : MonsterBehaviour
{
    private bool isReady = false;

    private Vector2 targetPos;
    private Vector2 targetDirection;
    private float curCooldown = 0;


    public override MonsterBehaviour DeepCopy()
    {
        return new ShootingSkillBehaviour();
    }

    public override void Update()
    {
        if (!isReady)
        {
            curCooldown += Time.deltaTime;
            CalculatePlayerDirection();
        }
        else
        {
            curCooldown = 0;
            Excute();
        }

    }
    
    private void CalculatePlayerDirection()
    {
        targetDirection = (targetTransform.position - monsterTransform.position).normalized;

        float attackRange = model.monsterStatus[(int)EMonsterStatus.MONSTER_ATTACKRANGE];

        float cooldownTime = model.monsterStatus[(int)EMonsterStatus.MONSTER_ATTACKSPEED];

        if (Vector2.Distance(targetPos, monsterTransform.position) < attackRange)
        {
            if (curCooldown >= cooldownTime)
            {
                isReady = true;
            }
        }
    }

    protected override void Excute()
    {
        OnStartSkillAction?.Invoke();

        ItemManager itemManager = ItemManager.getInstance;

        Projectile projectile = itemManager.GetProjectile();

        float attackRange = model.monsterStatus[(int)EMonsterStatus.MONSTER_ATTACKRANGE];

        damageData.direction = targetDirection;

        projectile.SetSprite("Tier2_Props_12");
        projectile.SetPrjectileInfo(damageData, monsterTransform.position, attackRange);
        projectile.SetTarget(target);

        isReady = false;

        //EndSkill
        OnEndSkillAction?.Invoke();
    }
}
