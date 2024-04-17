using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWeaponType
{
    PRECISE = 0,
    PRIMITIVE,
    MEDICAL,
    UNARMED,
    BLADE,
    BLUNT,
    HEAVY,
    ETHEART,
    TOOL,
    GUN,
    EXPLOSIVE,
    ELEMENTAL,
    SUPPORT,
    MEDIVAL,
    LEGEND,
    END = 15
}

public enum EWeaponStatus
{
    WEAPON_TIER = 0,
    FLAT_DAMAGE,
    TIER_BY_FLAT_DAMAGE_RATE,
    DAMAGE_MULTIPLIER,
    DAMAGE_MULTIPLIER_TYPE,
    TIER_BY_DAMAGE_MULTIPLIER_RATE,
    CRITICAL_CHANCE,
    TIER_BY_CRITICAL_CHANCE_RATE,
    COOLDOWN,
    TIER_BY_COOLDOWN_RATE,
    KNOCKBACK,
    ATTACK_RANGE,
    LIFE_STEAL,
    PIERCE,
    TIER_BY_PIERCE_RATE,
    BOUNCE,
    TIER_BY_BOUNCE_RATE,
    END
}

public class WeapomItemModel : BaseitemModel
{
    public EWeaponType[] weaponTypes;

}

public class WeaponStatus
{
    
}