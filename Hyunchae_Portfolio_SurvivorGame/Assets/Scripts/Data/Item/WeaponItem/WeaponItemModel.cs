using System.Collections;
using System.Collections.Generic;
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

public enum EWeaponAttackType
{
    STING = 0,
    SWING,
    SHOOT,
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

public class WeaponItemModel : BaseitemModel
{
    public EWeaponAttackType attackType;
    public WeaponStatus status;

}

public class WeaponStatus
{
    public float damage;
    public float cooldown;
    public float range;
    //status,Type, etc,,
}

public class WeaponItem
{
    private SpriteRenderer spriteRenderer;
    private WeaponItemModel itemModel;
    private Transform targetTransform;
    private Transform weaponTransform;
    private BaseWeaponAttack attackType;


    public void SetWeaponTransform(Transform _weaponTransform)
    {
        spriteRenderer = _weaponTransform.GetComponent<SpriteRenderer>();

        weaponTransform = _weaponTransform;
    }

    public void SetWeaponItemModel(WeaponItemModel _itemModel)
    {
        itemModel = _itemModel;

        spriteRenderer.sprite = ItemManager.getInstance.GetWeaponItemSprite(itemModel.itemUid);

        attackType = ItemManager.getInstance.GetAttackType(itemModel.attackType);

        attackType.SetModelInfo(itemModel);

        Debug.Log(attackType);

        attackType.SetInitPos(weaponTransform);
    }

    public void SetTarget(Transform _target)
    {
        targetTransform = _target;

        Debug.Log(attackType);
        Debug.Log(weaponTransform.name);

        attackType.SetTarget(_target);
    }

    public void Update()
    {
        if(itemModel == null || targetTransform == null)
        {
            return;
        }

        attackType.Update();
    }

    public void UnEquipWeapon()
    {
        ItemManager.getInstance.ReleaseWeaponAttackType(itemModel.attackType, attackType);
        attackType = null;
        itemModel = null;
        spriteRenderer.sprite = null;
    }
}