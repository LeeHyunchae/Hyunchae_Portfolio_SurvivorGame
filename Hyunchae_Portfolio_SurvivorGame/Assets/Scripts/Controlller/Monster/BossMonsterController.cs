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
    private BossMonsterModel model;

    private bool isDead = false;
    private float curBossHp;

    private float[] monsterStatusVariances = new float[(int)EMonsterStatus.END];

    public void Init(PlayerController _playerController)
    {
        myTransform = gameObject.transform;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        monsterManager = MonsterManager.getInstance;
        itemManager = ItemManager.getInstance;
        globalData = GlobalData.getInstance;

        //itemManager.OnRefreshEquipPassiveList += AddPassiveItem;
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

        InitRectCollisionCalculator();
    }

    public void SetModel(BossMonsterModel _model)
    {
        model = _model;

        spriteRenderer.sprite = Resources.Load<Sprite>(model.bossThumbnail);

        curBossHp = model.bossStatus[(int)EMonsterStatus.MONSTER_HP];

        patternSelector.SetBossStatus(model.bossStatus);

    }

    public void InitRectCollisionCalculator()
    {
        rectCollisionCalculator = new RectCollisionCalculator();
        rectCollisionCalculator.SetMyTargetable(this);
        rectCollisionCalculator.SetPlayerTargetable(target);

        rectCollisionCalculator.OnCollisionAction = OnCollisionPlayer;
    }

    //public void SetBossMonsterModel()

    private void Update()
    {
        if(pattern == null)
        {
            Debug.Log("Boss Patterns Null!!");
            SetPattern();
        }

        pattern.Update();
        rectCollisionCalculator.Update();
    }

    private void SetPattern()
    {
        pattern = patternSelector.GetPattern();
        pattern.OnEndPatternAction = OnEndPattern;
    }

    private void OnEndPattern()
    {
        pattern = null;
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
    }

    public void OnMonsterDie()
    {

    }

    private void OnCollisionPlayer()
    {
        //target.OnDamaged(damageData);
    }
}
