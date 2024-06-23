using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBossAttackBehaviour : IBossBehaviour
{
    protected ItemManager itemManager;

    protected Transform bossTransform;
    protected ITargetable target;
    protected Transform targetTransform;
    protected float attackRange;
    protected float moveSpeed;
    protected Vector2 direction;
    protected float distance;
    protected Vector2 pos;
    protected Vector2 dashStartPos;
    protected DamageData damageData;

    protected Action OnStartBehaviourAction;
    protected Action OnEndBehaviourAction;

    protected bool isEndBehaviour = false;

    public virtual void Init()
    {
        if (itemManager == null)
        {
            itemManager = ItemManager.getInstance;
        }

        isEndBehaviour = false;
    }
    
    public abstract void Excute();


    public virtual void SetBossTransform(Transform _transform)
    {
        bossTransform = _transform;
    }

    public virtual void SetStatus(float[] _status)
    {
        moveSpeed = _status[(int)EMonsterStatus.MONSTER_MOVESPEED];
        attackRange = _status[(int)EMonsterStatus.MONSTER_ATTACKRANGE];

        damageData = new DamageData()
        {
            damage = _status[(int)EMonsterStatus.MONSTER_DAMAGE]
        };
    }

    public virtual void SetTarget(ITargetable _target)
    {
        target = _target;
        targetTransform = _target.GetTransform();
    }

    public abstract void Update();

    public void SetOnStartBehaviourAction(Action _startAction)
    {
        OnStartBehaviourAction = _startAction;
    }

    public void SetOnEndBehaviourAction(Action _endAction)
    {
        OnEndBehaviourAction = _endAction;
    }
}
