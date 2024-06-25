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

    public List<BossPatternModel> bossPatternPhaseList;
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
    HEXAGONSHOOT,
    SEQUENCECIRCLESHOOT,
    END
}

public class JsonBossMonsterModel
{
    public int BossUid;
    public string BossName;
    public string BossThumbnail;
    public float[] BossStatus;
    public int DropPieceCount;
    public EMonsterLogicType FirstPhaseLogic;
    public EBossMonsterSkill[] FirstPhasePattern;
    public EMonsterLogicType SecondPhaseLogic;
    public EBossMonsterSkill[] SecondPhasePattern;
    public EMonsterLogicType ThirdPhaseLogic;
    public EBossMonsterSkill[] ThirdPhasePattern;
}