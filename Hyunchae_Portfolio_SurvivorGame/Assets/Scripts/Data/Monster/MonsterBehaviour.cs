using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBehaviour
{
    protected Transform monsterTransform; 
    protected Transform targetTransform;
    protected Vector2 pos;

    protected virtual void Excute() { }
    public virtual void Update() { }

    public virtual void SetMonsterTransform(Transform _transform)
    {
        monsterTransform = _transform;
    }
    public virtual void SetTarget(Transform _transform)
    {
        targetTransform = _transform;
    }

    public abstract void SetMonsterModel(MonsterModel _model);

    public abstract MonsterBehaviour DeepCopy();
}
