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

public class WeaponItemModel : Baseitem
{
    public WeaponStatus status;

}

public class WeaponStatus
{
    public float damage;
    public float attack_speed;

    //status,Type, etc,,
}

public class WeaponItem
{
    private GameObject weaponObj;
    private SpriteRenderer spriteRenderer;
    private WeaponItemModel itemModel;
    private Transform _transform;
    private Transform targetTransform;

    public Transform GetTransform => _transform;

    public void SetWeaponObject(GameObject _obj)
    {
        weaponObj = _obj;
        _transform = _obj.transform;
        spriteRenderer = _obj.GetComponent<SpriteRenderer>();
    }

    public void SetWeaponItemModel(WeaponItemModel _itemModel)
    {
        itemModel = _itemModel;

        //Todo Table
        spriteRenderer.sprite = ItemManager.getInstance.GetWeaponItemSprite(itemModel.itemUid);

    }

    public void SetTarget(Transform _target)
    {
        targetTransform = _target;
    }

    public void Update()
    {
        if(itemModel == null)
        {
            return;
        }

        RotatToTarget();
        Fire();
    }

    public void Fire()
    {

    }

    private void RotatToTarget()
    {
        if (targetTransform == null)
        {
            return;
        }

        Vector2 direction = (targetTransform.position - _transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        _transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}