using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeaponAttack
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
    protected Transform rotateTargetTransform;
    protected Vector2 targetPos;
    protected Vector2 targetDirection;
    protected Vector2 pos;
    protected ObbCollisionObject obbCollision;
    protected ITargetable[] targetMonsters;
    protected DamageData damageData;


    public void SetInitPos(Transform _initTransform)
    {
        weaponTransform = _initTransform;
        initPos = _initTransform.localPosition;
        pos = initPos;
    }

    public void RemoveTarget()
    {
        if(!isReady)
        {
            return;
        }
        rotateTargetTransform = null;
        weaponTransform.localPosition = initPos;
        weaponTransform.rotation = Quaternion.identity;
    }

    public virtual void SetModelInfo(WeaponItemModel _model)
    {
        cooldown = _model.status.cooldown;
        attackRange = _model.status.range;
    }

    public void SetObb(ObbCollisionObject _obb)
    {
        obbCollision = _obb;
    }

    public void SetDamageData(DamageData _damageData)
    {
        damageData = _damageData;
    }

    public void SetAttackTarget(ITargetable[] _targetMonsters)
    {
        targetMonsters = _targetMonsters;
    }

    public void SetRotateTarget(Transform _target)
    {
        if(!isReady)
        {
            return;
        }

        rotateTargetTransform = _target;
        targetPos = _target.position;
    }

    public void Update()
    {
        if (isReady)
        {
            curCooldown += Time.deltaTime;
            RotatToTarget();
        }
        else
        {
            Fire();
        }

    }

    protected abstract void ReadyFire();

    protected abstract void Fire();

    protected void EndFire()
    {
        isReady = true;
        curCooldown = 0;
        obbCollision.enabled = false;
    }
    protected void RotatToTarget()
    {
        if (rotateTargetTransform == null)
        {
            return;
        }

        targetDirection = (rotateTargetTransform.position - weaponTransform.position).normalized;

        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);

        weaponTransform.rotation = quaternion;

        if (Vector2.Distance(targetPos, weaponTransform.position) < attackRange)
        {
            if (curCooldown >= cooldown)
            {
                ReadyFire();
            }
        }
    }

    public abstract BaseWeaponAttack DeepCopy();

}

public class Sting : BaseWeaponAttack
{
    public override BaseWeaponAttack DeepCopy()
    {
        return new Sting();
    }

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

    protected override void ReadyFire()
    {
        isReady = false;
        obbCollision.enabled = true;
    }
}

public class Swing : BaseWeaponAttack
{
    public override BaseWeaponAttack DeepCopy()
    {
        return new Swing();
    }

    protected override void Fire()
    {
        
    }

    protected override void ReadyFire()
    {
        isReady = false;
        obbCollision.enabled = true;
    }
}

public class Shoot : BaseWeaponAttack
{
    private ItemManager itemManager = ItemManager.getInstance;
    private string bulletName;

    public override BaseWeaponAttack DeepCopy()
    {
        return new Shoot();
    }

    public override void SetModelInfo(WeaponItemModel _model)
    {
        base.SetModelInfo(_model);
        bulletName = _model.bulletImage;
    }

    protected override void Fire()
    {
        Projectile projectile = itemManager.GetProjectile();

        projectile.SetSprite(bulletName);

        damageData.direction = targetDirection;

        projectile.SetPrjectileInfo(damageData, weaponTransform.position, attackRange);
        projectile.SetTarget(targetMonsters);

        EndFire();
    }

    protected override void ReadyFire()
    {
        isReady = false;
    }
}
