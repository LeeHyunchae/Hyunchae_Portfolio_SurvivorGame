using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    private const int MAX_MONSTER_CAPACITY = 100;

    private const string SPRITELOADPATH = "Sprites/";
    private const string MONSTER_PARENT = "Monsters";

    private Dictionary<int, MonsterModel> monsterModelDict = new Dictionary<int, MonsterModel>();
    private List<MonsterModel> monsterModelList = new List<MonsterModel>();

    private Dictionary<int, Sprite> monsterSpriteDict = new Dictionary<int, Sprite>();

    private BaseMonsterBehaviourLogic[] logicTypeArr = new BaseMonsterBehaviourLogic[(int)EMonsterLogicType.END];
    private Dictionary<EMonsterLogicType, Queue<BaseMonsterBehaviourLogic>> logicTypeDict = new Dictionary<EMonsterLogicType, Queue<BaseMonsterBehaviourLogic>>();

    private MonsterBehaviour[] skillTypeArr = new MonsterBehaviour[(int)EMonsterSkillType.END];
    private Dictionary<EMonsterSkillType, Queue<MonsterBehaviour>> skillTypeDict = new Dictionary<EMonsterSkillType, Queue<MonsterBehaviour>>();

    private MonsterBehaviour[] moveTypeArr = new MonsterBehaviour[(int)EMonsterMoveType.END];
    private Dictionary<EMonsterMoveType, Queue<MonsterBehaviour>> moveTypeDict = new Dictionary<EMonsterMoveType, Queue<MonsterBehaviour>>();

    private LinkedList<MonsterController> aliveMonsterLinkedList = new LinkedList<MonsterController>();
    private Queue<LinkedListNode<MonsterController>> deadMonsterQueue = new Queue<LinkedListNode<MonsterController>>();

    private Transform parentTransform;

    public override bool Initialize()
    {
        LoadData();
        InitMonsterBehaviours();

        return base.Initialize();
    }

    public ITargetable[] CreateMonsterObjects()
    {
        parentTransform = new GameObject(MONSTER_PARENT).GetComponent<Transform>();

        MonsterController originMonster = Resources.Load<MonsterController>("Prefabs/Monster");

        ITargetable[] monsterArr = new ITargetable[MAX_MONSTER_CAPACITY];

        for(int i = 0; i<MAX_MONSTER_CAPACITY; i++)
        {
            MonsterController monster = GameObject.Instantiate<MonsterController>(originMonster,parentTransform);
            LinkedListNode<MonsterController> monsterNode = new LinkedListNode<MonsterController>(monster);
            deadMonsterQueue.Enqueue(monsterNode);
            monster.Init();
            monster.OnEnqueue();

            monsterArr[i] = monster;
        }

        return monsterArr;
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

    public MonsterController GetMonster()
    {
        MonsterController monster = null;

        if(deadMonsterQueue.Count == 0)
        {
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
}
