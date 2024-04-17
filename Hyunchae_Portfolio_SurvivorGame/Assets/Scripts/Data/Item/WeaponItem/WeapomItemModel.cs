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
    TIER_BY_FLAT_DAMAGE_MULTIPLIER,
    DAMAGE_MULTIPLIER,
    DAMAGE_MULTIPLIER_TYPE,
    TIER_BY_DAMAGE_MULTIPLIER_MULTIPLIER,
    CRITICAL_CHANCE,
    TIER_BY_CRITICAL_CHANCE_MULTIPLIER,
    COOLDOWN,
    TIER_BY_COOLDOWN_MULTIPLIER,
    KNOCKBACK,
    ATTACK_RANGE,
    LIFE_STEAL,
    PIERCE,
    TIER_BY_PIERCE_MULTIPLIER,
    BOUNCE,
    TIER_BY_BOUNCE_MULTIPLIER,
    END
}

public class WeapomItemModel : BaseitemModel
{
    public EWeaponType[] weaponTypes;

}

public class WeaponStatus
{
    
}