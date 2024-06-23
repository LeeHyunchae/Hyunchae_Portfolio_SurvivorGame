using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBossMoveBehaviour : IBossBehaviour
{
    protected Transform bossTransform;
    protected Transform targetTransform;
    protected float attackRange;
    protected float moveSpeed;
    protected Vector2 direction;
    protected float distance;
    protected Vector2 pos;
    protected Vector2 dashStartPos;

    protected Action OnStartBehaviourAction;
    protected Action OnEndBehaviourAction;

    protected bool isEndBehaviour = false;

    public virtual void Init()
    {
        isEndBehaviour = false;
    }

    public abstract void Excute();

    public void SetOnStartBehaviourAction(Action _startAction)
    {
        OnStartBehaviourAction = _startAction;
    }

    public void SetOnEndBehaviourAction(Action _endAction)
    {
        OnEndBehaviourAction = _endAction;
    }

    public virtual void SetBossTransform(Transform _transform)
    {
        bossTransform = _transform;
    }

    public virtual void SetStatus(float[] _status)
    {
        moveSpeed = _status[(int)EMonsterStatus.MONSTER_MOVESPEED];
        attackRange = _status[(int)EMonsterStatus.MONSTER_ATTACKRANGE];
    }

    public virtual void SetTarget(ITargetable _target)
    {
        targetTransform = _target.GetTransform();
    }

    public abstract void Update();

    
}
