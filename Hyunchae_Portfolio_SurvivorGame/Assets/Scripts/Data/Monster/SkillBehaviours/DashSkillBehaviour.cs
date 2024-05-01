using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkillBehaviour : MonsterBehaviour
{
    public override MonsterBehaviour DeepCopy()
    {
        return new DashSkillBehaviour();
    }

    public override void Update()
    {
        base.Update();
    }
}
