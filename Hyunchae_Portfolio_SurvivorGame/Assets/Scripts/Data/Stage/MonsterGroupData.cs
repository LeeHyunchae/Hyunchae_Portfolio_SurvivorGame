using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGroupData
{
    public int monsterGroupUID;
    public int waveTime;
    public List<MonsterSpawnData> monsterSpawnDatas;
}

public class MonsterSpawnData
{
    public int monsterUID;
    public int monsterCount;
    public float spawnStartTime;
    public float respawnCycleTile;
    public float spawnEndTime;
}