using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour , IPoolable
{

    private SpriteRenderer spriteRenderer;
    private MonsterModel monsterModel;
    private Transform _transform;
    private Transform targetTransform;
    private BaseMonsterBehaviourLogic behaviourLogic;

    private Action OnMonsterDieAction;

    public void Init()
    {
        _transform = this.transform;
        spriteRenderer = _transform.GetComponent<SpriteRenderer>();

        OnMonsterDieAction = MonsterManager.getInstance.OnMonsterDie;
    }

    public void SetMonsterModel(MonsterModel _monsterModel)
    {
        monsterModel = _monsterModel;
        Sprite sprite = MonsterManager.getInstance.GetMonsterSpriteToUid(monsterModel.monsterUid);

        spriteRenderer.sprite = sprite;

        SetMonsterBehaviour();
    }

    public void SetMonsterBehaviour()
    {
        behaviourLogic = MonsterManager.getInstance.GetBehaviourLogic(monsterModel.logicType);

        MonsterBehaviour skill = MonsterManager.getInstance.GetSkillBehaviour(monsterModel.skillType);
        if (skill != null)
        {
            skill.SetMonsterTransform(_transform);
            skill.SetTarget(targetTransform);
            behaviourLogic.SetSkillBehaviour(skill);
        }
        MonsterBehaviour move = MonsterManager.getInstance.GetMoveBehaviour(monsterModel.moveType);
        move.SetMonsterTransform(_transform);

        move.SetTarget(targetTransform);
        behaviourLogic.SetMoveBehaviour(move);


    }

    public void SetPlayerTransform(Transform _target)
    {
        targetTransform = _target;
    }

    public void Update()
    {
        if (monsterModel == null || targetTransform == null)
        {
            return;
        }

        behaviourLogic.Update();
    }

    public void OnEnqueue()
    {
        _transform.gameObject.SetActive(false);
    }

    public void OnDequeue()
    {
        _transform.gameObject.SetActive(true);
        OnMonsterDieAction?.Invoke();
    }
}
