using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonsterBehaviourLogic
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

    }
}
