using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviour
{
    protected Transform targetTransform;
    protected virtual void Excute() { }
    public virtual void Update() { }

    public virtual void SetTarget(Transform _transform)
    {
        targetTransform = _transform;
    }
}
