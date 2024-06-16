using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AugmentType
{
    MONSTERSPAWN = 100,

    PLAYER_MAXHP = 200,
    PLAYER_REGEN,
    PLAYER_LIFESTEAL,
    PLAYER_DAMAGE,
    PLAYER_MELEEDAMAGE,
    PLAYER_RANGEDAMAGE,
    PLAYER_ATTACKSPEED,
    PLAYER_CRITICALCHANCE,
    PLAYER_CRITICALDAMAGE,
    PLAYER_MOVESPEED,
    PLAYER_ENGINEERING,
    PLAYER_ATTACKRANGE,
    PLAYER_ARMOUR,
    PLAYER_EVASION,
    PLAYER_LUCK,
    PLAYER_PICKUPRANGE,
    PLAYER_HARVEST,

    MONSTER_HP = 300,
    MONSTER_MOVESPEED,
    MONSTER_DAMAGE,
    MONSTER_ATTACKSPEED,
    MONSTER_ATTACKRANGE
}

public class JsonAugmentData
{
    public int BuildUpID;
    public int BuildUpGrade;
    public string BuildUpName;
    public string BuildUpImage;
    public string BuildUpContent;
    public int BuildUpType;
    public int BuildUpVariable;
    public int BuildUpType2;
    public int BuildUpVariavle2;
    public int BuildUpGruop;
    public bool IsNotDuplicated;
}

public class AugmentData
{
    public int augmentUid;
    public int augmentGroup;
    public int augmentTier;
    public string augmentName;
    public string augmentImagePath;
    public string augmentInfo;
    public AugmentType firstAugmentType;
    public int firstAugmentValue;
    public AugmentType secondAugmentType;
    public int secondAugmentValue;
    public bool isNotDuplicated;
}
