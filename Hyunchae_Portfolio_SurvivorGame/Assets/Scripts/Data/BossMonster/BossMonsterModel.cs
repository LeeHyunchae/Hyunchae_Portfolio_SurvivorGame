using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterModel
{
    public int bossUid;
    public string bossName;
    public string bossThumbnail;
    public float[] bossStatus = new float[(int)EMonsterStatus.END];
    public int dropPieceCount;

    public List<BossPatternModel> bossPatternModels;
}

public class BossPatternModel
{
    public EMonsterLogicType logicType;
    public List<EBossMonsterSkill> skillList;
}

public enum EBossMonsterSkill
{
    DASH = 0,
    CIRCLESHOOT,
    TRIPLESHOOT,
    FOLLOWMOVE,
    END
}