using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSkillBehaviour : MonsterBehaviour
{
    public override MonsterBehaviour DeepCopy()
    {
        return new ShootingSkillBehaviour();
    }

    public override void Update()
    {
        base.Update();
    }
}
