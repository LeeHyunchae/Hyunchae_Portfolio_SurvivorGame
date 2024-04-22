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
    private Transform _transform;
    private Transform targetTransform;

    public Transform GetTransform => _transform;

    private float curCooldown;
    private Vector2 targetDirection;
    private Vector3 originPos;
    private bool isAttackReady;

    public void SetWeaponTransform(Transform _weaponTransform)
    {
        _transform = _weaponTransform;
        spriteRenderer = _transform.GetComponent<SpriteRenderer>();

        SetOriginPosition(_weaponTransform);
    }

    public void SetWeaponItemModel(WeaponItemModel _itemModel)
    {
        itemModel = _itemModel;

        spriteRenderer.sprite = ItemManager.getInstance.GetWeaponItemSprite(itemModel.itemUid);

        curCooldown = itemModel.status.cooldown;
    }

    public void SetOriginPosition(Transform _originTransform)
    {
        originPos = _originTransform.position;
        _transform.position = originPos;
    }

    public void SetTarget(Transform _target)
    {
        targetTransform = _target;
    }

    public void Update()
    {
        if(itemModel == null || targetTransform == null)
        {
            return;
        }

        RotatToTarget();


    }

    //public void Fire()
    //{
    //    if(isAttackReady)
    //    {
    //        curCooldown += Time.deltaTime;
    //    }

    //    if (curCooldown >= itemModel.status.cooldown)
    //    {
    //        isAttackReady = false;
    //        Debug.Log(isAttackReady);

    //        switch (itemModel.attackType)
    //        {
    //            case EWeaponAttackType.STING:
    //                Sting();
    //                break;
    //            case EWeaponAttackType.SWING:
    //                Swing();
    //                break;
    //            case EWeaponAttackType.SHOOT:
    //                Shoot();
    //                break;
    //        }
    //    }
    //}

    private void RotatToTarget()
    {
        if (targetTransform == null)
        {
            return;
        }

        targetDirection = (targetTransform.position - _transform.position).normalized;

        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;

        _transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    //private void Sting()
    //{
    //    float pingPong = Mathf.PingPong(Time.deltaTime * 10, 2 * 2) - 2;
    //    //Debug.Log(pingPong);
    //    _transform.position = originPos + Vector3.forward * pingPong;

    //    //Debug.Log("Sting!");

    //    //Todo damage


    //    if(_transform.position == originPos)
    //    {
    //        EndFire();
    //    }
    //}

    //private void Swing()
    //{
    //    Debug.Log("Swing!");
    //}

    //private void Shoot()
    //{
    //    Debug.Log("Shoot!");
    //}

    //private void EndFire()
    //{
    //    curCooldown = 0;
    //    isAttackReady = true;
    //    Debug.Log(isAttackReady);
    //}
}