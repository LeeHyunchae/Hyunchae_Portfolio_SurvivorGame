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
    private RectCollisionCalculator rectCollisionCalculator;

    private ITargetable target;


    public Transform GetMonsterTransform => myTransform;

    private MonsterManager monsterManager;

    private bool isDead = true;

    private DamageData damageData;

    private float curMonsterHp;

    public void Init(PlayerController _playerController)
    {
        myTransform = gameObject.transform;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        monsterManager = MonsterManager.getInstance;
        OnMonsterDieAction = monsterManager.OnMonsterDie;

        NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        targetTransform = _playerController.GetTransform();
        target = _playerController;

        InitRectCollisionCalculator();
    }

    public void InitRectCollisionCalculator()
    {
        rectCollisionCalculator = new RectCollisionCalculator();
        rectCollisionCalculator.SetMyTargetable(this);
        rectCollisionCalculator.SetPlayerTargetable(target);

        rectCollisionCalculator.OnCollisionAction = OnCollisionPlayer;
    }

    public void SetMonsterModel(MonsterModel _monsterModel)
    {
        monsterModel = _monsterModel;
        Sprite sprite = monsterManager.GetMonsterSpriteToUid(monsterModel.monsterUid);

        spriteRenderer.sprite = sprite;

        damageData = new DamageData()
        {
            damage = monsterModel.monsterStatus[(int)EMonsterStatus.MONSTER_DAMAGE],
            knockback = 0
        };

        curMonsterHp = monsterModel.monsterStatus[(int)EMonsterStatus.MONSTER_HP];

        SetMonsterBehaviour();

    }

    public void SetStatusVariance(Monster_Status_Variance _variance)
    {

    }

    public void SetMonsterBehaviour()
    {
        behaviourLogic = monsterManager.GetBehaviourLogic(monsterModel.logicType);

        MonsterBehaviour skill = monsterManager.GetSkillBehaviour(monsterModel.skillType);
        skill.SetMonsterTransform(myTransform);
        skill.SetTarget(targetTransform,target);
        skill.SetMonsterModel(monsterModel);
        skill.SetDamageData(damageData);
        behaviourLogic.SetSkillBehaviour(skill);

        MonsterBehaviour move = monsterManager.GetMoveBehaviour(monsterModel.moveType);
        move.SetMonsterTransform(myTransform);
        move.SetTarget(targetTransform);
        move.SetMonsterModel(monsterModel);

        behaviourLogic.SetMoveBehaviour(move);
    }

    private void Update()
    {
        if (monsterModel == null || targetTransform == null)
        {
            return;
        }

        behaviourLogic.Update();
        rectCollisionCalculator.Update();
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

    public void OnDamaged(DamageData _damageData)
    {
        curMonsterHp -= _damageData.damage;

        Debug.Log("Monster On Damage : " + _damageData.damage);
        Debug.Log("Monster HP : " + curMonsterHp);

        if (curMonsterHp <= 0)
        {
            OnDieMonster();
        }

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

    private void OnCollisionPlayer()
    {
        target.OnDamaged(damageData);
    }
}
