using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceBehaviourLogic : BaseMonsterBehaviourLogic
{
    public override BaseMonsterBehaviourLogic DeepCopy()
    {
        return new SequenceBehaviourLogic();
    }

    public override void Update()
    {
        base.Update();
    }
}
