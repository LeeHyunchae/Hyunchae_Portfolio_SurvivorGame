using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonsterBehaviourLogic
{
    private MonsterBehaviour skillBehaviour;
    private MonsterBehaviour moveBehaviour;

    public void SetSkillBehaviour(MonsterBehaviour _behaviour)
    {
        skillBehaviour = _behaviour;
    }

    public void SetMoveBehaviour(MonsterBehaviour _behaviour)
    {
        moveBehaviour = _behaviour;
    }

    public virtual void Update()
    {
        if(skillBehaviour != null)
        {
            skillBehaviour.Update();
        }
            moveBehaviour.Update();
    }

    public abstract BaseMonsterBehaviourLogic DeepCopy();
}
