using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EMonsterState
{
    RUN,
    DEAD,
    SPAWNWAIT
}

public class MonsterController : MonoBehaviour , ITargetable
{
    private const string ANIM_MONSTER_ID = "MonsterID";
    private const string ANIM_MONSTER_STATE = "MonsterState";
    private const float DAMAGETIME = 0.25f;
    private const float SPAWNWAITTIME = 1;
    private const float DEADTIME = 1f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
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
    private SoundManager soundManager;

    private bool isDead = true;
    private float curDeadTime = 0;

    private DamageData damageData = new DamageData();

    private float curMonsterHp;

    private float[] monsterStatusVariances = new float[(int)EMonsterStatus.END];

    private MonsterBehaviour skillBehaviour;
    private MonsterBehaviour moveBehaviour;

    private bool isDamaged = false;
    private float curDamagedTime = 0;

    private bool isSpawnWait = false;
    private float curSpawnWaitTime = 0;

    private bool isDeadWait = false;

    public void Init(PlayerController _playerController)
    {
        myTransform = gameObject.transform;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        
        monsterManager = MonsterManager.getInstance;
        itemManager = ItemManager.getInstance;
        globalData = GlobalData.getInstance;
        soundManager = SoundManager.getInstance;

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
        
        SetMonsterBehaviour();

        SetStatusToModel();

        animator.SetInteger(ANIM_MONSTER_ID, monsterModel.monsterUid);
        animator.SetInteger(ANIM_MONSTER_STATE, (int)EMonsterState.SPAWNWAIT);

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
        if (globalData.GetPause || monsterModel == null || targetTransform == null)
        {
            return;
        }


        if (isSpawnWait)
        {
            SpawnWait();
            return;
        }
        else if (isDeadWait)
        {
            DeadWait();
            return;
        }

        behaviourLogic.Update();
        rectCollisionCalculator.Update();

        if (isDamaged)
        {
            SwapColor();
        }
    }

    private void SpawnWait()
    {
        curSpawnWaitTime += Time.deltaTime;

        if(curSpawnWaitTime >= SPAWNWAITTIME)
        {
            curSpawnWaitTime = 0;
            isSpawnWait = false;
            isDead = false;

            animator.SetInteger(ANIM_MONSTER_STATE, (int)EMonsterState.RUN);

        }
    }

    private void SwapColor()
    {
        curDamagedTime += Time.deltaTime;

        spriteRenderer.color = Color.yellow;

        if(curDamagedTime >= DAMAGETIME)
        {
            spriteRenderer.color = Color.white;
            curDamagedTime = 0;
            isDamaged = false;
        }
    }

    private void DeadWait()
    {
        curDeadTime += Time.deltaTime;

        if(curDeadTime >= DEADTIME)
        {
            curDeadTime = 0;
            isDeadWait = false;
            ReleaseData();
        }    
    }

    public void OnEnqueue()
    {
        gameObject.SetActive(false);
        isDead = true;
    }

    public void OnDequeue()
    {
        gameObject.SetActive(true);
        spriteRenderer.color = Color.white;
        isSpawnWait = true;

    }

    private void OnMonsterDie()
    {
        animator.SetInteger(ANIM_MONSTER_STATE, (int)EMonsterState.DEAD);
        isDead = true;
        isDeadWait = true;
        spriteRenderer.color = Color.white;
        curDamagedTime = 0;
        isDamaged = false;
        globalData.IncreasePieceCount(monsterModel.dropPieceCount);

    }

    private void ReleaseData()
    {
        monsterManager.ReleaseBehaviourLogic(monsterModel.logicType, behaviourLogic);
        monsterManager.ReleaseSkillBehaviour(monsterModel.skillType, skillBehaviour);
        monsterManager.ReleaseMoveBehaviour(monsterModel.moveType, moveBehaviour);

        OnMonsterDieAction?.Invoke(this);

        monsterModel = null;

        animator.SetInteger(ANIM_MONSTER_STATE, (int)EMonsterState.SPAWNWAIT);
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public void OnDamaged(DamageData _damageData)
    {
        curMonsterHp -= _damageData.damage;

        isDamaged = true;

        Vector2 pos = myTransform.position;

        Vector2 direction = _damageData.direction.normalized;

        pos.x += direction.x * _damageData.knockback;
        pos.y += direction.y * _damageData.knockback;

        myTransform.position = pos;

        if (curMonsterHp <= 0)
        {
            OnMonsterDie();
        }


        int soundIndex = Random.Range((int)EAudioClip.MELEE_ONE, (int)EAudioClip.SELECT);

        soundManager.PlaySFX((EAudioClip)soundIndex);
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
