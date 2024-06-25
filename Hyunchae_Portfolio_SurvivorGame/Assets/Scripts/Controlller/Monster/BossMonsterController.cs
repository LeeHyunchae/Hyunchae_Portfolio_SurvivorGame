using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMonsterController : MonoBehaviour ,ITargetable
{
    private GlobalData globalData;
    private MonsterManager monsterManager;
    private ItemManager itemManager;

    private Transform myTransform;
    private SpriteRenderer spriteRenderer;
    private Transform targetTransform;
    private ITargetable target;
    private BossPatternSelector patternSelector;
    private RectCollisionCalculator rectCollisionCalculator;
    private BossPattern pattern;
    private BossMonsterModel bossMonsterModel;
    private HpBarController hpBar;
    private DamageData damageData;

    private bool isDead = false;
    private float curBossHp;

    private int curPhaseCount = 0;

    private float[] monsterStatusVariances = new float[(int)EMonsterStatus.END];

    public void Init(PlayerController _playerController)
    {
        myTransform = gameObject.transform;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        monsterManager = MonsterManager.getInstance;
        itemManager = ItemManager.getInstance;
        globalData = GlobalData.getInstance;

        itemManager.OnRefreshEquipPassiveList += AddPassiveItem;
        //OnMonsterDieAction = monsterManager.OnMonsterDie;

        NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        targetTransform = _playerController.GetTransform();
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

        spriteRenderer.sprite = monsterManager.GetBossMonsterSpriteToUid(bossMonsterModel.bossUid);

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
        hpBar.UpdatePos(myTransform.position);

        pattern.Update();
        rectCollisionCalculator.Update();
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

        if(curBossHp <= 0)
        {
            OnMonsterDie();
        }

        CheckNextPhase();

        hpBar.SetHPBarFillAmount(curBossHp / bossMonsterModel.bossStatus[(int)EMonsterStatus.MONSTER_HP]);

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
        isDead = true;
        SetActive(false);
        hpBar.SetActive(false);
        globalData.IncreasePieceCount(bossMonsterModel.dropPieceCount);
    }

    private void OnCollisionPlayer()
    {
        target.OnDamaged(damageData);
    }

    public void SetActive(bool _isActive) => gameObject.SetActive(_isActive);
    
    public void SpawnBoss()
    {
        isDead = false;
        SetActive(true);
        hpBar.SetActive(true);
        SetPattern();
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
