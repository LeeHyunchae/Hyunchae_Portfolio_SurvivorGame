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
    public float[] monsterStatus = new float[(int)EMonsterStatus.END];
    public int dropPieceCount;
}

public enum EMonsterStatus
{
    MONSTER_HP = 0,
    MONSTER_MOVESPEED,
    MONSTER_DAMAGE ,
    MONSTER_ATTACKSPEED,
    MONSTER_ATTACKRANGE,
    END
}

public class MonsterStatusVariance
{
    public EMonsterStatus monsterStatus;
    public float variance;
}
