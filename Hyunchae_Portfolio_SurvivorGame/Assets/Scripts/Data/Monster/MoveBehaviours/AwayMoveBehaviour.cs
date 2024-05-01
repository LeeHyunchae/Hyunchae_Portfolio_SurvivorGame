using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwayMoveBehaviour : MonsterBehaviour
{
    public override MonsterBehaviour DeepCopy()
    {
        return new AwayMoveBehaviour();
    }

    public override void Update()
    {
        base.Update();
    }
}
