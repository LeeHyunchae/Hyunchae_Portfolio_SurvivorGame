using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//public enum EWeaponType
//{
//    PRECISE = 0,
//    PRIMITIVE,
//    MEDICAL,
//    UNARMED,
//    BLADE,
//    BLUNT,
//    HEAVY,
//    ETHEART,
//    TOOL,
//    GUN,
//    EXPLOSIVE,
//    ELEMENTAL,
//    SUPPORT,
//    MEDIVAL,
//    LEGEND,
//    END = 15
//}

public enum EWeaponType
{
    STING = 0,
    SWING,
    SHOOT,
    END
}

public enum EWeaponAttackType
{
    NONE = 0,
    PIERCE,
    BOUNCE,
    END
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

public class WeaponItemModel : BaseItemModel
{
    public EWeaponType WeaponType;
    public int weaponGroup;
    public int weaponTier;
    public int weaponSenergy;
    public WeaponStatus status;

    public StringBuilder GetWeaponInfo()
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.Append("Tier : " + weaponTier + 1 + "\n\n");

        if (status.damage != 0)
        { stringBuilder.Append("Damage : "+ status.damage + "\n"); }
        if(status.criticalChance != 0)
        { stringBuilder.Append("CriticalChance : " + status.criticalChance + "\n"); }
        if (status.flatDamage != 0)
        { stringBuilder.Append("FlatDamage" + status.flatDamage + "\n"); }
        if (status.range != 0)
        { stringBuilder.Append("Range : " + status.range + "\n"); }
        if (status.speed != 0)
        { stringBuilder.Append("Speed : " + status.speed + "\n"); }
        if (status.cooldown != 0)
        { stringBuilder.Append("CoolDown : " + status.cooldown + "\n"); }
        if (status.knockback != 0)
        { stringBuilder.Append("Knockback : " + status.knockback); }

        return stringBuilder;
    }
}

public class WeaponStatus
{
    public EWeaponAttackType attackType;
    public float damage;
    public float criticalChance;
    public float flatDamage;
    public float range;
    public float speed;
    public float cooldown;
    public float knockback;
    //status,Type, etc,,
}

public class JsonWeaponData
{
    public int WeaponID;
    public int WeaponGroup;
    public int WeaponTier;
    public int WeaponSynergy;
    public EWeaponType WeaponType;
    public EWeaponAttackType WeaponAttackType;
    public float WeaponDamage;
    public float WeaponCritical;
    public float WeaponTypeDamage;
    public float WeaponRange;
    public float WeaponSpeed;
    public float WeaponCoolDown;
    public float WeaponKnockback;
    public float WeaponStatusEffect;
    public string ItemImage;
    public string BulletName;
    public string ItemName;
    
}