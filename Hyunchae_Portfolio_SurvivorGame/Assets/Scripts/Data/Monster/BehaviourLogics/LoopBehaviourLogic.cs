using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBehaviourLogic : BaseMonsterBehaviourLogic
{
    public override void Update()
    {
        base.Update();
    }

    public override BaseMonsterBehaviourLogic DeepCopy()
    {
        return new LoopBehaviourLogic();
    }
}
