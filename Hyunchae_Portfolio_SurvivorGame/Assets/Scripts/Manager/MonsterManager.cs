using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    private const int MAX_MONSTER_CAPACITY = 100;

    private const string SPRITELOADPATH = "Sprites/";
    private const string MONSTER_PARENT = "Monsters";

    private Dictionary<int, MonsterModel> monsterModelDict = new Dictionary<int, MonsterModel>();
    private List<MonsterModel> monsterModelList = new List<MonsterModel>();

    private List<BossMonsterModel> bossMonsterModelList = new List<BossMonsterModel>();
    private Dictionary<int, BossMonsterModel> bossMonsterModelDict = new Dictionary<int, BossMonsterModel>();

    private Dictionary<int, Sprite> monsterSpriteDict = new Dictionary<int, Sprite>();
    private Dictionary<int, Sprite> bossMonsterSpriteDict = new Dictionary<int, Sprite>();

    private BaseMonsterBehaviourLogic[] logicTypeArr = new BaseMonsterBehaviourLogic[(int)EMonsterLogicType.END];
    private Dictionary<EMonsterLogicType, Queue<BaseMonsterBehaviourLogic>> logicTypeDict = new Dictionary<EMonsterLogicType, Queue<BaseMonsterBehaviourLogic>>();

    private MonsterBehaviour[] skillTypeArr = new MonsterBehaviour[(int)EMonsterSkillType.END];
    private Dictionary<EMonsterSkillType, Queue<MonsterBehaviour>> skillTypeDict = new Dictionary<EMonsterSkillType, Queue<MonsterBehaviour>>();

    private MonsterBehaviour[] moveTypeArr = new MonsterBehaviour[(int)EMonsterMoveType.END];
    private Dictionary<EMonsterMoveType, Queue<MonsterBehaviour>> moveTypeDict = new Dictionary<EMonsterMoveType, Queue<MonsterBehaviour>>();

    private LinkedList<MonsterController> aliveMonsterLinkedList = new LinkedList<MonsterController>();
    private Queue<LinkedListNode<MonsterController>> deadMonsterQueue = new Queue<LinkedListNode<MonsterController>>();

    private Transform parentTransform;
    private PlayerController player;

    private BossMonsterController bossMonster;

    public override bool Initialize()
    {
        LoadData();
        InitMonsterBehaviours();

        return base.Initialize();
    }

    private void InitMonsterBehaviours()
    {
        logicTypeArr[(int)EMonsterLogicType.SEQUENCE] = new SequenceBehaviourLogic();
        logicTypeArr[(int)EMonsterLogicType.LOOP] = new LoopBehaviourLogic();

        logicTypeDict[EMonsterLogicType.SEQUENCE] = new Queue<BaseMonsterBehaviourLogic>();
        logicTypeDict[EMonsterLogicType.LOOP] = new Queue<BaseMonsterBehaviourLogic>();

        skillTypeArr[(int)EMonsterSkillType.NONE] = new NoneSkillBehabviour();
        skillTypeArr[(int)EMonsterSkillType.SHOOTING] = new ShootingSkillBehaviour();
        skillTypeArr[(int)EMonsterSkillType.DASH] = new DashSkillBehaviour();

        skillTypeDict[EMonsterSkillType.NONE] = new Queue<MonsterBehaviour>();
        skillTypeDict[EMonsterSkillType.SHOOTING] = new Queue<MonsterBehaviour>();
        skillTypeDict[EMonsterSkillType.DASH] = new Queue<MonsterBehaviour>();

        moveTypeArr[(int)EMonsterMoveType.FOLLOW] = new FollowMoveBehaviour();
        moveTypeArr[(int)EMonsterMoveType.AWAY] = new AwayMoveBehaviour();

        moveTypeDict[EMonsterMoveType.FOLLOW] = new Queue<MonsterBehaviour>();
        moveTypeDict[EMonsterMoveType.AWAY] = new Queue<MonsterBehaviour>();
    }

    private void LoadData()
    {
        monsterModelList = TableLoader.LoadFromFile<List<MonsterModel>>("Monster/TestMonster");

        int count = monsterModelList.Count;

        for(int i = 0;  i < count; i++)
        {
            MonsterModel model = monsterModelList[i];

            monsterModelDict.Add(model.monsterUid, model);
            Sprite monsterSprite = Resources.Load<Sprite>(SPRITELOADPATH + model.monsterThumbnail);
            monsterSpriteDict.Add(model.monsterUid, monsterSprite);
        }

        List<JsonBossMonsterModel> jsonBossMonsterModels = TableLoader.LoadFromFile<List<JsonBossMonsterModel>>("Monster/TestBossMonster");

        count = jsonBossMonsterModels.Count;

        for(int i = 0; i <count; i ++)
        {
            JsonBossMonsterModel jsonBossModel = jsonBossMonsterModels[i];
            BossMonsterModel bossModel = new BossMonsterModel()
            {
                bossUid = jsonBossModel.BossUid,
                bossName = jsonBossModel.BossName,
                bossThumbnail = jsonBossModel.BossThumbnail,
                bossStatus = jsonBossModel.BossStatus,
                dropPieceCount = jsonBossModel.DropPieceCount,
                bossPatternPhaseList = new List<BossPatternModel>()
            };

            BossPatternModel patternModel = new BossPatternModel();
            patternModel.logicType = jsonBossModel.FirstPhaseLogic;
            patternModel.skillList = jsonBossModel.FirstPhasePattern.ToList();
            bossModel.bossPatternPhaseList.Add(patternModel);

            patternModel = new BossPatternModel();
            patternModel.logicType = jsonBossModel.SecondPhaseLogic;
            patternModel.skillList = jsonBossModel.SecondPhasePattern.ToList();
            bossModel.bossPatternPhaseList.Add(patternModel);

            patternModel = new BossPatternModel();
            patternModel.logicType = jsonBossModel.ThirdPhaseLogic;
            patternModel.skillList = jsonBossModel.ThirdPhasePattern.ToList();
            bossModel.bossPatternPhaseList.Add(patternModel);

            bossMonsterModelList.Add(bossModel);
            bossMonsterModelDict.Add(bossModel.bossUid, bossModel);

            Sprite bossSprite = Resources.Load<Sprite>(SPRITELOADPATH + bossModel.bossThumbnail);
            bossMonsterSpriteDict.Add(bossModel.bossUid, bossSprite);
        }
    }

    public void SetPlayer(PlayerController _playerController)
    {
        player = _playerController;
    }

    public ITargetable[] CreateMonsterObjects()
    {
        aliveMonsterLinkedList.Clear();
        deadMonsterQueue.Clear();

        parentTransform = new GameObject(MONSTER_PARENT).GetComponent<Transform>();

        MonsterController originMonster = Resources.Load<MonsterController>("Prefabs/Monster");

        ITargetable[] monsterArr = new ITargetable[MAX_MONSTER_CAPACITY + 1];

        for (int i = 0; i < MAX_MONSTER_CAPACITY; i++)
        {
            MonsterController monster = GameObject.Instantiate<MonsterController>(originMonster, parentTransform);
            LinkedListNode<MonsterController> monsterNode = new LinkedListNode<MonsterController>(monster);
            deadMonsterQueue.Enqueue(monsterNode);
            monster.Init(player);
            monster.OnEnqueue();

            monsterArr[i] = monster;
        }

        return monsterArr;
    }


    public ITargetable CreateBossObject(int _bossUid)
    {
        bossMonsterModelDict.TryGetValue(_bossUid, out BossMonsterModel bossMonsterModel);

        if(bossMonsterModel == null)
        {
            Debug.Log("Wrong Boss Uid");
        }

        BossMonsterController originMonster = Resources.Load<BossMonsterController>("Prefabs/BossMonster");

        bossMonster = GameObject.Instantiate<BossMonsterController>(originMonster);
        bossMonster.Init(player);
        bossMonster.SetModel(bossMonsterModel);
        bossMonster.SetActive(false);

        return bossMonster;
    }

    public MonsterModel GetMonsterModelToUid(int _uid)
    {
        monsterModelDict.TryGetValue(_uid, out MonsterModel model);

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

    public Sprite GetBossMonsterSpriteToUid(int _uid)
    {
        bossMonsterSpriteDict.TryGetValue(_uid, out Sprite bossSprite);

#if UNITY_EDITOR

        if (bossSprite == null)
        {
            Debug.Log("Not Exist Sprite");
        }
#endif

        return bossSprite;
    }

    public MonsterController GetMonster()
    {
        MonsterController monster = null;

        // 죽은 몬스터가 하나도 없이 몬스터가 모두 스폰 되었을 때
        if(deadMonsterQueue.Count == 0)
        {
            // 가장 최초에 스폰된 몬스터를 수거하고 다시 반환
            LinkedListNode<MonsterController> firstMonsterNode = aliveMonsterLinkedList.First;
            aliveMonsterLinkedList.RemoveFirst();
            aliveMonsterLinkedList.AddLast(firstMonsterNode);

            monster = firstMonsterNode.Value;
        }
        else
        {
            LinkedListNode<MonsterController> monsterNode = deadMonsterQueue.Dequeue();
            aliveMonsterLinkedList.AddLast(monsterNode);

            monster = monsterNode.Value;
        }

        monster.OnDequeue();

        return monster;
    }

    public LinkedList<MonsterController> GetAllAliveMonsters => aliveMonsterLinkedList;

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

    public void ReleaseBehaviourLogic(EMonsterLogicType _logicType,BaseMonsterBehaviourLogic _logic)
    {
        logicTypeDict[_logicType].Enqueue(_logic);
    }

    public void ReleaseSkillBehaviour(EMonsterSkillType _skillType, MonsterBehaviour _behaviour)
    {
        skillTypeDict[_skillType].Enqueue(_behaviour);
    }

    public void ReleaseMoveBehaviour(EMonsterMoveType _moveType, MonsterBehaviour _behaviour)
    {
        moveTypeDict[_moveType].Enqueue(_behaviour);
    }

    public void OnMonsterDie(MonsterController _monster)
    {
        MonsterController deadMonster = _monster;
        LinkedListNode<MonsterController> currentNode = aliveMonsterLinkedList.First;
        while (currentNode != null)
        {
            if (currentNode.Value == deadMonster)
            {
                deadMonsterQueue.Enqueue(currentNode);
                deadMonster.OnEnqueue();
                break;
            }

            currentNode = currentNode.Next;
        }

        aliveMonsterLinkedList.Remove(currentNode);
    }

    public void ReleaseAllAliveMonster()
    {
        LinkedListNode<MonsterController> currentNode = aliveMonsterLinkedList.First;
        while (currentNode != null)
        {
            LinkedListNode<MonsterController> nextNode = currentNode.Next;

            deadMonsterQueue.Enqueue(currentNode);
            currentNode.Value.OnEnqueue();
            aliveMonsterLinkedList.Remove(currentNode);
            currentNode = nextNode;
        }
    }

    public BossMonsterController GetBossMonster()
    {
        return bossMonster;
    }
}

#region SampleCode
//.. SampleCode
public class BaseMonsterInfo
{
    public int hp;
    public int atk;
    public int def;
}

public class MonsterData
{
    public BaseMonsterInfo info;
}

public class MonsterObject
{
    public MonsterData data;
}


public interface IAffectionData
{
    IAffectionData Get();
}

public class AffactionData : IAffectionData
{
    public IAffectionData Get()
    {
        return this;
    }
}

public interface IAffectionController
{
}

public class AffectionItemController : IAffectionController
{

}


public class UpgradeData : IAffectionData
{
    public IAffectionData Get()
    {
        return this;
    }
}

public class UpgradeControlelr : IAffectionController
{

}


public class BuffManger : Singleton<BuffManger>
{
    private Dictionary<System.Type, IAffectionController> affectionDict = null;

    public void AddValue<T>(int key, int val) where T : IAffectionController, new()
    {
        System.Type type = typeof(T);
        if(affectionDict.TryGetValue(type, out var affectionController) == false)
        {
            affectionController = new T();
            affectionDict.Add(type, affectionController);
        }

        //affectionController.Add(
    }
}

#endregion