using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSkillBehaviour : MonsterBehaviour
{
    private bool isReady = false;

    private Vector2 targetPos;
    private Vector2 targetDirection;
    private ObbCollisionObject obbCollision;
    private ITargetable[] targetMonsters;
    private float curCooldown;


    public override MonsterBehaviour DeepCopy()
    {
        return new ShootingSkillBehaviour();
    }

    public override void SetMonsterModel(MonsterModel _model)
    {
        base.SetMonsterModel(_model);
        curCooldown = model.status.cooldown;
    }

    public override void Update()
    {
        //if (!isReady)
        //{
        //    curCooldown += Time.deltaTime;
        //    CalculatePlayerDirection();
        //}
        //else
        //{
        //    curCooldown = 0;
        //    Excute();
        //}

    }
    
    private void CalculatePlayerDirection()
    {
        targetDirection = (targetTransform.position - monsterTransform.position).normalized;

        if (Vector2.Distance(targetPos, monsterTransform.position) < model.status.attackRange)
        {
            if (curCooldown >= model.status.cooldown)
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

        projectile.SetSprite("Exp 0");
        projectile.SetPrjectileInfo(targetDirection, model.status.damage, monsterTransform.position, model.status.attackRange);
        //projectile.SetTarget(targetMonsters);


        //EndSkill
        OnEndSkillAction?.Invoke();
    }
}
