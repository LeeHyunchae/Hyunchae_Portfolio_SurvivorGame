using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonsterBehaviourLogic
{
    protected MonsterBehaviour skillBehaviour;
    protected MonsterBehaviour moveBehaviour;

    public virtual void SetSkillBehaviour(MonsterBehaviour _behaviour)
    {
        skillBehaviour = _behaviour;
    }

    public virtual void SetMoveBehaviour(MonsterBehaviour _behaviour)
    {
        moveBehaviour = _behaviour;
    }

    public abstract void Update();

    public abstract BaseMonsterBehaviourLogic DeepCopy();
}
