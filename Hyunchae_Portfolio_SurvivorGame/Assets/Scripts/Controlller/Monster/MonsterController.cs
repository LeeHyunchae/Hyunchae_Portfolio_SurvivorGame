using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour , ITargetable
{

    private SpriteRenderer spriteRenderer;
    private MonsterModel monsterModel;
    private Transform myTransform;
    private Transform targetTransform;
    private BaseMonsterBehaviourLogic behaviourLogic;

    private Action<MonsterController> OnMonsterDieAction;

    public Transform GetMonsterTransform => myTransform;

    private MonsterManager monsterManager;

    private bool isDead = true;

    public void Init()
    {
        myTransform = gameObject.transform;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        monsterManager = MonsterManager.getInstance;
        OnMonsterDieAction = monsterManager.OnMonsterDie;

        NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
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

        MonsterBehaviour skill = monsterManager.GetSkillBehaviour(monsterModel.skillType);
        skill.SetMonsterTransform(myTransform);
        skill.SetTarget(targetTransform);
        skill.SetMonsterModel(monsterModel);
        behaviourLogic.SetSkillBehaviour(skill);

        MonsterBehaviour move = monsterManager.GetMoveBehaviour(monsterModel.moveType);
        move.SetMonsterTransform(myTransform);
        move.SetTarget(targetTransform);
        move.SetMonsterModel(monsterModel);

        behaviourLogic.SetMoveBehaviour(move);
    }

    public void SetPlayerTransform(Transform _target)
    {
        targetTransform = _target;
    }

    public void SetMonsterPosition(Vector2 _pos)
    {
        myTransform.position = _pos;
    }

    private void Update()
    {
        if (monsterModel == null || targetTransform == null)
        {
            return;
        }

        behaviourLogic.Update();
    }

    public void OnEnqueue()
    {
        gameObject.SetActive(false);
        isDead = true;
    }

    public void OnDequeue()
    {
        gameObject.SetActive(true);
        isDead = false;
    }

    private void OnDieMonster()
    {
        isDead = true;
        OnMonsterDieAction?.Invoke(this);
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public void OnDamaged()
    {
        Debug.Log("Monster Weapon Collision");
        OnDieMonster();
    }
    public Bounds GetSpriteBounds()
    {
        return spriteRenderer.bounds;
    }

    public Vector2 GetPosition()
    {
        return myTransform.position;
    }

    public Transform GetTransform()
    {
        return myTransform;
    }
}
