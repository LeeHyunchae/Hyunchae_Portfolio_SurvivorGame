using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMonsterAttackType
{
    MELEE = 0,
    RANGE,
    DASH,
    BOSS
}

public class MonsterModel
{
    public int monsterUid;
    public string monsterName;
    public string monsterThumbnail;

    public EMonsterAttackType attackType;
    public MonsterStatus status;
}

public class MonsterStatus
{
    public float maxHP;
    public float damage;
    public float cooldown;
    public float moveSpeed;
    public float attackRange;
}
