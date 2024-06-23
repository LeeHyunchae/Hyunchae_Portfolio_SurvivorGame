using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossBehaviour
{
    public void Init();
    public void SetBossTransform(Transform _transform);
    public void SetTarget(ITargetable _target);
    public void SetStatus(float[] _statuses);

    public void Update();
    public void Excute();

    public void SetOnStartBehaviourAction(Action _startAction);
    public void SetOnEndBehaviourAction(Action _endAction);
}
