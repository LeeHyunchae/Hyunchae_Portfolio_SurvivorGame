using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttackType
{
    protected float attackRange;
    protected float cooldown;
    protected float damage;

    protected bool isReady;
    protected Vector2 initPos;
    protected float curCooldown;
    protected Transform weaponTransform;
    protected Transform targetTransform;

    public void SetInitPos(Transform _initTransform)
    {
        weaponTransform = _initTransform;
        initPos = _initTransform.position;
    }

    public void SetInfo(WeaponItemModel _model)
    {
        cooldown = _model.status.cooldown;
        attackRange = _model.status.range;
    }

    public void SetTarget(Transform _target)
    {
        targetTransform = _target;
    }

    public void Update()
    {
        if (isReady)
        {
            curCooldown += Time.deltaTime;
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
}

public class Sting : BaseAttackType
{
    private float pingpong;

    protected override void Fire()
    {
        isReady = false;

        pingpong = Mathf.PingPong(Time.deltaTime * 1, 2 * 2) - 2;

        weaponTransform.position = initPos + Vector2.up * pingpong;

    }
}