using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMonsterController : MonoBehaviour ,ITargetable
{
    private const string ANIM_BOSSMONSTER_ID = "BossMonsterID";
    private const string ANIM_BOSSMONSTER_STATE = "BossMonsterState";
    private const float DAMAGETIME = 0.25f;
    private const float SPAWNWAITTIME = 1;
    private const float DEADTIME = 1f;

    private GlobalData globalData;
    private ItemManager itemManager;
    private SoundManager soundManager;

    private Transform myTransform;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private ITargetable target;
    private BossPatternSelector patternSelector;
    private RectCollisionCalculator rectCollisionCalculator;
    private BossPattern pattern;
    private BossMonsterModel bossMonsterModel;
    private HpBarController hpBar;
    private DamageData damageData;

    private bool isDead = true;
    private float curDeadTime = 0;
    private float curBossHp;

    private int curPhaseCount = 0;

    private float[] monsterStatusVariances = new float[(int)EMonsterStatus.END];

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

        itemManager = ItemManager.getInstance;
        globalData = GlobalData.getInstance;
        soundManager = SoundManager.getInstance;

        itemManager.OnRefreshEquipPassiveList += AddPassiveItem;
        //OnMonsterDieAction = monsterManager.OnMonsterDie;

        NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        target = _playerController;

        patternSelector = new BossPatternSelector();
        patternSelector.Init();
        patternSelector.SetBossTransform(myTransform);
        patternSelector.SetTarget(_playerController);

        damageData = new DamageData();

        InitRectCollisionCalculator();
    }

    public void InitRectCollisionCalculator()
    {
        rectCollisionCalculator = new RectCollisionCalculator();
        rectCollisionCalculator.SetMyTargetable(this);
        rectCollisionCalculator.SetPlayerTargetable(target);

        rectCollisionCalculator.OnCollisionAction = OnCollisionPlayer;
    }
    public void SetModel(BossMonsterModel _model)
    {
        bossMonsterModel = _model;

        curBossHp = bossMonsterModel.bossStatus[(int)EMonsterStatus.MONSTER_HP];

        patternSelector.SetBossPhase(bossMonsterModel.bossPatternPhaseList);

        SetStatusApplyToModel();

    }

    public void SetHPBar(HpBarController _hpBar)
    {
        hpBar = _hpBar;
        hpBar.SetActive(false);
        hpBar.Init();
    }
    private void Update()
    {
        if (globalData.GetPause)
        {
            return;
        }

        if (isSpawnWait)
        {
            SpawnWait();
            return;
        }
        else if(isDeadWait)
        {
            DeadWait();
            return;
        }

        hpBar.UpdatePos(myTransform.position);

        pattern.Update();
        rectCollisionCalculator.Update();

        if(isDamaged)
        {
            SwapColor();
        }
    }

    private void SpawnWait()
    {
        curSpawnWaitTime += Time.deltaTime;

        if (curSpawnWaitTime >= SPAWNWAITTIME)
        {
            Debug.Log("SpawnBoss");
            animator.SetInteger(ANIM_BOSSMONSTER_STATE, (int)EMonsterState.RUN);

            curSpawnWaitTime = 0;
            isSpawnWait = false;
            isDead = false;
            hpBar.SetActive(true);
            SetPattern();
        }
    }

    private void SwapColor()
    {
        curDamagedTime += Time.deltaTime;

        spriteRenderer.color = Color.yellow;

        if (curDamagedTime >= DAMAGETIME)
        {
            spriteRenderer.color = Color.white;
            curDamagedTime = 0;
            isDamaged = false;
        }
    }
    private void DeadWait()
    {
        curDeadTime += Time.deltaTime;

        if (curDeadTime >= DEADTIME)
        {
            isDeadWait = false;
            curDeadTime = 0;
            animator.SetInteger(ANIM_BOSSMONSTER_STATE, (int)EMonsterState.SPAWNWAIT);

            ReleaseData();
        }
    }

    private void SetPattern()
    {
        pattern = patternSelector.GetPattern(curPhaseCount);
        pattern.OnEndPatternAction = OnEndPattern;
    }

    private void OnEndPattern()
    {
        pattern.InitPatterns();
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public Vector2 GetPosition()
    {
        return myTransform.position;
    }

    public Bounds GetSpriteBounds()
    {
        return spriteRenderer.bounds;
    }

    public Transform GetTransform()
    {
        return myTransform;
    }

    public void OnDamaged(DamageData _damageData)
    {
        curBossHp -= _damageData.damage;

        isDamaged = true;

        if(curBossHp <= 0)
        {
            OnMonsterDie();
        }

        CheckNextPhase();

        hpBar.SetHPBarFillAmount(curBossHp / bossMonsterModel.bossStatus[(int)EMonsterStatus.MONSTER_HP]);

        int soundIndex = Random.Range((int)EAudioClip.MELEE_ONE, (int)EAudioClip.SELECT);

        soundManager.PlaySFX((EAudioClip)soundIndex);
    }

    private void CheckNextPhase()
    {
        if(curBossHp <= bossMonsterModel.bossStatus[(int)EMonsterStatus.MONSTER_HP] * 0.7f && curPhaseCount == 0)
        {
            curPhaseCount++;
            SetPattern();
        }
        else if(curBossHp <= bossMonsterModel.bossStatus[(int)EMonsterStatus.MONSTER_HP] * 0.4f && curPhaseCount == 1)
        {
            curPhaseCount++;
            SetPattern();
        }
    }

    public void OnMonsterDie()
    {
        animator.SetInteger(ANIM_BOSSMONSTER_STATE, (int)EMonsterState.DEAD);
        isDead = true;
        isDeadWait = true;
        hpBar.SetActive(false);
        globalData.IncreasePieceCount(bossMonsterModel.dropPieceCount);
    }

    private void ReleaseData()
    {
        SetActive(false);
    }

    private void OnCollisionPlayer()
    {
        target.OnDamaged(damageData);
    }

    public void SetActive(bool _isActive) => gameObject.SetActive(_isActive);
    
    public void SpawnBoss()
    {
        SetActive(true);

        animator.SetInteger(ANIM_BOSSMONSTER_ID, bossMonsterModel.bossUid);
        animator.SetInteger(ANIM_BOSSMONSTER_STATE, (int)EMonsterState.SPAWNWAIT);
        isSpawnWait = true;
    }

    public void AddPassiveItem()
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

    private void SetStatusVariance(MonsterStatusVariance _variance)
    {
        monsterStatusVariances[(int)_variance.monsterStatus] += _variance.variance;
    }

    private void SetStatusApplyToModel()
    {
        int count = monsterStatusVariances.Length;

        float[] tempMonsterStatus = new float[(int)EMonsterStatus.END];

        for (int i = 0; i < count; i++)
        {
            tempMonsterStatus[i] = monsterStatusVariances[i] + bossMonsterModel.bossStatus[i];
        }

        damageData.damage = tempMonsterStatus[(int)EMonsterStatus.MONSTER_DAMAGE];

        curBossHp = tempMonsterStatus[(int)EMonsterStatus.MONSTER_HP];

        patternSelector.SetBossStatus(bossMonsterModel.bossStatus);

    }
}
