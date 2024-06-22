using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPassiveItemType
{
    ATTACKABLE = 0,
    SUPPORTABLE
}

public class PassiveItemModel : BaseItemModel
{
    public EPassiveItemType itemPassiveType;
    public int itemTier;
    public List<ItemStatusVariance> status_Variances;
    public string itemInfo;
}

public class JsonPassiveItemModel
{
    public int ItemID;
    public int ItemTier;
    public EPassiveItemType ItemPassiveType;
    public List<ItemStatusVariance> ItemStatusEffect;
    public string ItemImage;
    public string BulletName;
    public string ItemName;
    public string ItemContent;
    public int ItemPrice;
}

public enum EItemStatusType
{
    PLAYER_MAXHP = 100,
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

    MONSTER_MAXHP = 200,
    MONSTER_MOVESPEED,
    MONSTER_DAMAGE,
    MONSTER_ATTACKSPEED,
    MONSTER_ATTACKRANGE
}

public enum EItemStatusTarget
{
    PLAYER = 1,
    MONSTER
}

public class ItemStatusVariance
{
    public EItemStatusType itemStatusType;
    public bool isRatio;
    public float variance;
}