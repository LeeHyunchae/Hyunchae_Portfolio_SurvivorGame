using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkillBehaviour : MonsterBehaviour
{
    public override MonsterBehaviour DeepCopy()
    {
        return new DashSkillBehaviour();
    }

    public override void SetMonsterModel(MonsterModel _model)
    {
        base.SetMonsterModel(_model);
    }

    public override void Update()
    {

    }

    protected override void Excute()
    {
        OnStartSkillAction?.Invoke();


        //EndSkill
        OnEndSkillAction?.Invoke();
    }
}
