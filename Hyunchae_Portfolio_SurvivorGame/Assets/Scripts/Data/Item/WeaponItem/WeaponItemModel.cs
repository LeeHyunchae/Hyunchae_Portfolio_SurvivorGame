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
    SHOOT
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

    private Vector3 originPos;

    private BaseAttack attackType;
    private Sting stingAttack;
    private Swing swingAttack;
    private Shoot shootAttack;

    public WeaponItem()
    {
        stingAttack = new Sting();
        swingAttack = new Swing();
        shootAttack = new Shoot();
    }

    public void SetWeaponTransform(Transform _weaponTransform)
    {
        spriteRenderer = _weaponTransform.GetComponent<SpriteRenderer>();

        stingAttack.SetInitPos(_weaponTransform);
        swingAttack.SetInitPos(_weaponTransform);
        shootAttack.SetInitPos(_weaponTransform);
    }

    public void SetWeaponItemModel(WeaponItemModel _itemModel)
    {
        itemModel = _itemModel;

        spriteRenderer.sprite = ItemManager.getInstance.GetWeaponItemSprite(itemModel.itemUid);

        switch(itemModel.attackType)
        {
            case EWeaponAttackType.STING:
                attackType = stingAttack;
                break;
            case EWeaponAttackType.SWING:
                attackType = swingAttack;
                break;
            case EWeaponAttackType.SHOOT:
                attackType = shootAttack;
                break;
        }

        attackType.SetInfo(itemModel);
    }

    public void SetTarget(Transform _target)
    {
        targetTransform = _target;

        stingAttack.SetTarget(_target);
        swingAttack.SetTarget(_target);
        shootAttack.SetTarget(_target);
    }

    public void Update()
    {
        if(itemModel == null || targetTransform == null)
        {
            return;
        }

        attackType.Update();
    }
}