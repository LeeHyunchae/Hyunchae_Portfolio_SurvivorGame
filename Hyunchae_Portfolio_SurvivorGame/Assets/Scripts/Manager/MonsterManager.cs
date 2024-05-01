using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    private string SPRITELOADPATH = "Sprites/";

    private Dictionary<int, MonsterModel> monsterDict = new Dictionary<int, MonsterModel>();
    private List<MonsterModel> monsterModels = new List<MonsterModel>();

    private Dictionary<int, Sprite> monsterSpriteDict = new Dictionary<int, Sprite>();

    private ObjectPool<MonsterController> monsterPool;

    private BaseMonsterBehaviourLogic[] logicTypeArr = new BaseMonsterBehaviourLogic[(int)EMonsterLogicType.END];
    private Dictionary<EMonsterLogicType, Queue<BaseMonsterBehaviourLogic>> logicTypeDict = new Dictionary<EMonsterLogicType, Queue<BaseMonsterBehaviourLogic>>();

    private MonsterBehaviour[] skillTypeArr = new MonsterBehaviour[(int)EMonsterSkillType.END];
    private Dictionary<EMonsterSkillType, Queue<MonsterBehaviour>> skillTypeDict = new Dictionary<EMonsterSkillType, Queue<MonsterBehaviour>>();

    private MonsterBehaviour[] moveTypeArr = new MonsterBehaviour[(int)EMonsterMoveType.END];
    private Dictionary<EMonsterMoveType, Queue<MonsterBehaviour>> moveTypeDict = new Dictionary<EMonsterMoveType, Queue<MonsterBehaviour>>();

    public override bool Initialize()
    {
        LoadData();
        InitMonsterBehaviours();

        return base.Initialize();
    }

    private void InitMonsterPool()
    {
        monsterPool = PoolManager.getInstance.GetObjectPool<MonsterController>();
        monsterPool.Init("Prefabs/Monster", 20);
    }

    private void InitMonsterBehaviours()
    {
        logicTypeArr[(int)EMonsterLogicType.SEQUENCE] = new SequenceBehaviourLogic();
        logicTypeArr[(int)EMonsterLogicType.LOOP] = new LoopBehaviourLogic();

        logicTypeDict[EMonsterLogicType.SEQUENCE] = new Queue<BaseMonsterBehaviourLogic>();
        logicTypeDict[EMonsterLogicType.LOOP] = new Queue<BaseMonsterBehaviourLogic>();

        skillTypeArr[(int)EMonsterSkillType.NONE] = null;
        skillTypeArr[(int)EMonsterSkillType.SHOOTING] = new ShootingSkillBehaviour();
        skillTypeArr[(int)EMonsterSkillType.DASH] = new DashSkillBehaviour();

        skillTypeDict[EMonsterSkillType.NONE] = null;
        skillTypeDict[EMonsterSkillType.SHOOTING] = new Queue<MonsterBehaviour>();
        skillTypeDict[EMonsterSkillType.DASH] = new Queue<MonsterBehaviour>();

        moveTypeArr[(int)EMonsterMoveType.FOLLOW] = new FollowMoveBehaviour();
        moveTypeArr[(int)EMonsterMoveType.AWAY] = new AwayMoveBehaviour();

        moveTypeDict[EMonsterMoveType.FOLLOW] = new Queue<MonsterBehaviour>();
        moveTypeDict[EMonsterMoveType.AWAY] = new Queue<MonsterBehaviour>();
    }

    private void LoadData()
    {
        monsterModels = TableLoader.LoadFromFile<List<MonsterModel>>("Monster/TestMonster");

        int count = monsterModels.Count;

        for(int i = 0;  i < count; i++)
        {
            MonsterModel model = monsterModels[i];

            monsterDict.Add(model.monsterUid, model);
            Sprite monsterSprite = Resources.Load<Sprite>(SPRITELOADPATH + model.monsterThumbnail);
            monsterSpriteDict.Add(model.monsterUid, monsterSprite);
        }
    }

    public MonsterModel GetMonsterModelToUid(int _uid)
    {
        monsterDict.TryGetValue(_uid, out MonsterModel model);

#if UNITY_EDITOR

        if (model == null)
        {
            Debug.Log("Not Exist Monster");
        }
#endif

        return model;

    }

    public Sprite GetMonsterSpriteToUid(int _uid)
    {
        monsterSpriteDict.TryGetValue(_uid, out Sprite monsterSprite);

#if UNITY_EDITOR

        if (monsterSprite == null)
        {
            Debug.Log("Not Exist Sprite");
        }
#endif

        return monsterSprite;
    }

    public MonsterController GetMonster()
    {
        if(monsterPool == null)
        {
            InitMonsterPool();
        }

        return monsterPool.GetObject();
    }

    public BaseMonsterBehaviourLogic GetBehaviourLogic(EMonsterLogicType _logicType)
    {
        if(logicTypeDict[_logicType].Count == 0)
        {
            BaseMonsterBehaviourLogic logicType = logicTypeArr[(int)_logicType].DeepCopy();
            logicTypeDict[_logicType].Enqueue(logicType);
        }

        return logicTypeDict[_logicType].Dequeue();
    }

    public MonsterBehaviour GetSkillBehaviour(EMonsterSkillType _skillType)
    {
        if(skillTypeDict[_skillType] == null)
        {
            return null;
        }


        if (skillTypeDict[_skillType].Count == 0)
        {
            MonsterBehaviour skill = skillTypeArr[(int)_skillType].DeepCopy();
            skillTypeDict[_skillType].Enqueue(skill);
        }

        return skillTypeDict[_skillType].Dequeue();
    }

    public MonsterBehaviour GetMoveBehaviour(EMonsterMoveType _moveType)
    {

        if (moveTypeDict[_moveType].Count == 0)
        {
            MonsterBehaviour move = moveTypeArr[(int)_moveType].DeepCopy();
            moveTypeDict[_moveType].Enqueue(move);
        }

        return moveTypeDict[_moveType].Dequeue();
    }

    public void OnMonsterDie()
    {

    }
}
