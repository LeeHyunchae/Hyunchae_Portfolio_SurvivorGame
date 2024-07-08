using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �ൿ�� �����ϴ� Ŭ�������� �θ� Ŭ����
public abstract class MonsterBehaviour
{
    protected Transform monsterTransform; 
    protected Transform targetTransform;
    protected Vector2 pos;
    protected Action OnStartSkillAction;
    protected Action OnEndSkillAction;
    protected ITargetable target;
    protected DamageData damageData;
    protected float[] monsterStatus;

    protected abstract void Excute();
    public abstract void Update();

    public Action GetOnSkillAction => OnStartSkillAction;

    public virtual void SetMonsterTransform(Transform _transform)
    {
        monsterTransform = _transform;
    }
    public virtual void SetTarget(Transform _transform, ITargetable _target)
    {
        targetTransform = _transform;
        target = _target;
    }

    public virtual void SetTarget(Transform _transform)
    {
        targetTransform = _transform;
    }

    public virtual void SetMonsterStatus(float[] _monsterStatus)
    {
        monsterStatus = _monsterStatus;
    }

    public abstract MonsterBehaviour DeepCopy();

    public virtual void SetDamageData(DamageData _damageData)
    {
        damageData = _damageData;
    }
}
