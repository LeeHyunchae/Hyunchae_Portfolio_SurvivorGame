using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBehaviourLogic : BaseMonsterBehaviourLogic
{
    public override void Update()
    {
        skillBehaviour.Update();

        moveBehaviour.Update();
    }

    public override BaseMonsterBehaviourLogic DeepCopy()
    {
        return new LoopBehaviourLogic();
    }
}
