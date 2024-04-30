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

    public override bool Initialize()
    {
        LoadData();

        return base.Initialize();
    }

    private void InitMonsterPool()
    {
        monsterPool = PoolManager.getInstance.GetObjectPool<MonsterController>();
        monsterPool.Init("Prefabs/Monster", 20);
    }

    private void LoadData()
    {
        monsterModels = TableLoader.LoadFromFile<List<MonsterModel>>("Monster/TestMonster");

        int count = monsterModels.Count;

        for(int i = 0;  i < count; i++)
        {
            MonsterModel model = monsterModels[i];

            monsterDict.Add(model.monsterUid, model);
            monsterSpriteDict.Add(model.monsterUid, Resources.Load<Sprite>(SPRITELOADPATH + model.monsterName));

        }
    }

    public MonsterController GetMonster()
    {
        if(monsterPool == null)
        {
            InitMonsterPool();
        }

        return monsterPool.GetObject();
    }
}
