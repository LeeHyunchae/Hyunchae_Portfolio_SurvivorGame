using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneSkillBehabviour : MonsterBehaviour
{
    public override MonsterBehaviour DeepCopy()
    {
        return new NoneSkillBehabviour();
    }

    public override void Update()
    {
        // is NoneAction Monster only
        return;
    }

    protected override void Excute()
    {
        OnStartSkillAction?.Invoke();


        //EndSkill
        OnEndSkillAction?.Invoke();
    }
}
