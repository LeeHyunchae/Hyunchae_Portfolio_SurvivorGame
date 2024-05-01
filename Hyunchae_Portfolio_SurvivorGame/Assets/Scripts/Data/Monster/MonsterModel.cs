using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMonsterSkillType
{
    NONE,
    SHOOTING,
    DASH,
    END
}

public enum EMonsterMoveType
{
    FOLLOW = 0,
    AWAY,
    END
}

public enum EMonsterLogicType
{
    SEQUENCE = 0,
    LOOP,
    END
}

public class MonsterModel
{
    public int monsterUid;
    public string monsterName;
    public string monsterThumbnail;

    public EMonsterLogicType logicType;
    public EMonsterSkillType skillType;
    public EMonsterMoveType moveType;
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
