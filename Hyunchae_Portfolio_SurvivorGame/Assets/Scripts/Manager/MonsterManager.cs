using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    private Dictionary<int, MonsterModel> monsterDict = new Dictionary<int, MonsterModel>();

    public override bool Initialize()
    {
        return base.Initialize();
    }

    private void LoadData()
    {

    }
}
