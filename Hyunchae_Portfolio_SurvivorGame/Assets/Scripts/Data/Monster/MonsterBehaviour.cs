using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBehaviour
{
    protected Transform monsterTransform; 
    protected Transform targetTransform;
    protected Vector2 pos;
    protected Action OnStartSkillAction;
    protected Action OnEndSkillAction;

    protected abstract void Excute();
    public abstract void Update();

    public Action GetOnSkillAction => OnStartSkillAction;

    protected MonsterModel model;

    public virtual void SetMonsterTransform(Transform _transform)
    {
        monsterTransform = _transform;
    }
    public virtual void SetTarget(Transform _transform)
    {
        targetTransform = _transform;
    }

    public virtual void SetMonsterModel(MonsterModel _model)
    {
        model = _model;
    }

    public abstract MonsterBehaviour DeepCopy();
}
