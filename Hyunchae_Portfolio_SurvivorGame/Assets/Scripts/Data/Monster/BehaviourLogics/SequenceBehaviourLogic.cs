using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceBehaviourLogic : BaseMonsterBehaviourLogic
{
    private bool isAction = false;

    public override BaseMonsterBehaviourLogic DeepCopy()
    {
        return new SequenceBehaviourLogic();
    }

    public override void Update()
    {
        skillBehaviour.Update();

        if (!isAction)
        {
            moveBehaviour.Update();
        }
    }
}
