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

    private GlobalData globalData;
    private MonsterManager monsterManager;
    private ItemManager itemManager;

    private bool isDead = true;

    private DamageData damageData = new DamageData();

    private float curMonsterHp;

    private float[] monsterStatusVariances = new float[(int)EMonsterStatus.END];

    private MonsterBehaviour skillBehaviour;
    private MonsterBehaviour moveBehaviour;

    public void Init(PlayerController _playerController)
    {
        myTransform = gameObject.transform;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        monsterManager = MonsterManager.getInstance;
        itemManager = ItemManager.getInstance;
        globalData = GlobalData.getInstance;

        itemManager.OnRefreshEquipPassiveList += AddPassiveItem;
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

        SetMonsterBehaviour();

        SetStatusToModel();
        
    }

    public void SetStatusToModel()
    {
        int count = monsterStatusVariances.Length;

        float[] tempMonsterStatus = new float[(int)EMonsterStatus.END];

        for(int i = 0; i < count; i ++)
        {
           tempMonsterStatus[i] = monsterStatusVariances[i] + monsterModel.monsterStatus[i];
        }

        damageData.damage = tempMonsterStatus[(int)EMonsterStatus.MONSTER_DAMAGE];

        curMonsterHp = tempMonsterStatus[(int)EMonsterStatus.MONSTER_HP];

        skillBehaviour.SetMonsterStatus(tempMonsterStatus);
        skillBehaviour.SetDamageData(damageData);
        moveBehaviour.SetMonsterStatus(tempMonsterStatus);

    }

    public void SetStatusVariance(MonsterStatusVariance _variance)
    {
        monsterStatusVariances[(int)_variance.monsterStatus] += _variance.variance;
    }

    public void SetMonsterBehaviour()
    {
        behaviourLogic = monsterManager.GetBehaviourLogic(monsterModel.logicType);

        skillBehaviour = monsterManager.GetSkillBehaviour(monsterModel.skillType);
        skillBehaviour.SetMonsterTransform(myTransform);
        skillBehaviour.SetTarget(targetTransform,target);

        behaviourLogic.SetSkillBehaviour(skillBehaviour);

        moveBehaviour = monsterManager.GetMoveBehaviour(monsterModel.moveType);
        moveBehaviour.SetMonsterTransform(myTransform);
        moveBehaviour.SetTarget(targetTransform);

        behaviourLogic.SetMoveBehaviour(moveBehaviour);
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

        monsterManager.ReleaseBehaviourLogic(monsterModel.logicType, behaviourLogic);
        monsterManager.ReleaseSkillBehaviour(monsterModel.skillType, skillBehaviour);
        monsterManager.ReleaseMoveBehaviour(monsterModel.moveType, moveBehaviour);

        globalData.IncreasePieceCount(monsterModel.dropPieceCount);

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

    private void AddPassiveItem()
    {
        List<BaseItemModel> itemList = itemManager.GetAllEquipPassiveItemModelList;

        int slotIndex = itemList.Count - 1;

        PassiveItemModel itemModel = itemList[slotIndex] as PassiveItemModel;

        int varianceCount = itemModel.status_Variances.Count;

        List<ItemStatusVariance> statusVariances = itemModel.status_Variances;

        for (int i = 0; i < varianceCount; i++)
        {
            ItemStatusVariance itemStatusVariance = statusVariances[i];
            int isMonsterStatus = (int)itemStatusVariance.itemStatusType / 100;

            if (isMonsterStatus != (int)EItemStatusTarget.MONSTER)
            {
                continue;
            }

            MonsterStatusVariance statusVariance = new MonsterStatusVariance()
            {
                monsterStatus = (EMonsterStatus)((int)itemStatusVariance.itemStatusType % 100),
                variance = itemStatusVariance.variance
            };

            SetStatusVariance(statusVariance);
        }
    }
}
