using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttack
{
    protected const float STING_SPEED = 15;

    protected float attackRange;
    protected float cooldown;
    protected float damage;
    protected float curCooldown;

    protected bool isReady = true;
    protected bool isReturn = false;

    protected Vector2 initPos;
    protected Transform weaponTransform;
    protected Transform targetTransform;
    protected Vector2 targetPos;
    protected Vector2 targetDirection;
    protected Vector2 pos;


    public void SetInitPos(Transform _initTransform)
    {
        weaponTransform = _initTransform;
        initPos = _initTransform.localPosition;
        pos = initPos;
    }

    public void SetModelInfo(WeaponItemModel _model)
    {
        cooldown = _model.status.cooldown;
        attackRange = _model.status.range;
    }

    public void SetTarget(Transform _target)
    {
        targetTransform = _target;
        targetPos = _target.position;
    }

    public void Update()
    {
        if (isReady)
        {
            curCooldown += Time.deltaTime;
            RotatToTarget();
        }

        if (curCooldown >= cooldown)
        {
            isReady = false;
            Fire();
        }
    }

    protected abstract void Fire();

    protected void EndFire()
    {
        isReady = true;
        curCooldown = 0;
    }
    protected void RotatToTarget()
    {
        if (targetTransform == null)
        {
            return;
        }

        targetDirection = (targetTransform.position - weaponTransform.position).normalized;

        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);

        weaponTransform.rotation = quaternion;
    }
}

public class Sting : BaseAttack
{
    protected override void Fire()
    {
        if(!isReturn)
        {
            pos.x += targetDirection.x * Time.deltaTime * STING_SPEED;
            pos.y += targetDirection.y * Time.deltaTime * STING_SPEED;

            weaponTransform.localPosition = pos;

            if(Vector2.Distance(weaponTransform.localPosition,initPos) >=  attackRange)
            {
                isReturn = true;
            }
        }
        else
        {
            pos.x += -targetDirection.x * Time.deltaTime * STING_SPEED;
            pos.y += -targetDirection.y * Time.deltaTime * STING_SPEED;

            weaponTransform.localPosition = pos;

            if(Vector2.Distance(weaponTransform.localPosition , initPos) <= 0.5f)
            {
                weaponTransform.localPosition = initPos;
                pos = initPos;
                isReturn = false;
                EndFire();
            }
        }
         
    }
}

public class Swing : BaseAttack
{
    protected override void Fire()
    {
        throw new System.NotImplementedException();
    }
}

public class Shoot : BaseAttack
{
    private ItemManager itemManager = ItemManager.getInstance;

    protected override void Fire()
    {
        Projectile projectile = itemManager.GetProjectile();

        projectile.SetSprite("as");
        projectile.SetPrjectileInfo(targetDirection, damage, weaponTransform.position, attackRange);

        EndFire();
    }
}
