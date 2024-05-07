using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private MonsterModel monsterModel;
    private Transform _transform;
    private Transform targetTransform;
    private BaseMonsterBehaviourLogic behaviourLogic;

    private Action<MonsterController> OnMonsterDieAction;

    public Transform GetMonsterTransform => _transform;

    private MonsterManager monsterManager;

    public void Init()
    {
        _transform = this.transform;
        spriteRenderer = _transform.GetComponent<SpriteRenderer>();

        monsterManager = MonsterManager.getInstance;
        OnMonsterDieAction = monsterManager.OnMonsterDie;
    }

    public void SetMonsterModel(MonsterModel _monsterModel)
    {
        monsterModel = _monsterModel;
        Sprite sprite = monsterManager.GetMonsterSpriteToUid(monsterModel.monsterUid);

        spriteRenderer.sprite = sprite;

        SetMonsterBehaviour();
    }

    public void SetMonsterBehaviour()
    {
        behaviourLogic = monsterManager.GetBehaviourLogic(monsterModel.logicType);

        Debug.Log(monsterModel.skillType);
        MonsterBehaviour skill = monsterManager.GetSkillBehaviour(monsterModel.skillType);
        skill.SetMonsterTransform(_transform);
        skill.SetTarget(targetTransform);
        skill.SetMonsterModel(monsterModel);
        behaviourLogic.SetSkillBehaviour(skill);

        MonsterBehaviour move = monsterManager.GetMoveBehaviour(monsterModel.moveType);
        move.SetMonsterTransform(_transform);
        move.SetTarget(targetTransform);
        move.SetMonsterModel(monsterModel);

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
    }

    private void OnDieMonster()
    {
        OnMonsterDieAction?.Invoke(this);
    }
}
