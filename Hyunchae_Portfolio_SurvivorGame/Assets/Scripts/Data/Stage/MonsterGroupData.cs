using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonMonsterGroupData
{
    public int MonsterGroupID;
    public int MonsterID;
    public int MonsterNumber;
    public float FirstSpawnTime;
    public float RespawnCycle;
    public float EndSpawnTime;
}

public class MonsterGroupData
{
    public int monsterGroupUID;
    public List<MonsterSpawnData> monsterSpawnDatas;
}

public class MonsterSpawnData
{
    public int monsterUID;
    public int monsterCount;
    public float spawnStartTime;
    public float respawnCycleTime;
    public float spawnEndTime;
    public bool isSpawnStart;
}