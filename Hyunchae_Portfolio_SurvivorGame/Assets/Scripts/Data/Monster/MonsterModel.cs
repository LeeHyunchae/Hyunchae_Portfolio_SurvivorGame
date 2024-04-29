using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMonsterAttackType
{
    MELEE = 0,
    RANGE,
    DASH,
    BOSS
}

public class MonsterModel
{
    public int monsterUid;
    public string monsterName;
    public string monsterThumbnail;

    public EMonsterAttackType attackType;
    public MonsterStatus status;
}

public class MonsterStatus
{
    public float maxHP;
    public float damage;
    public float cooldown;
    public float moveSpeed;
    public float attackRange;
}

public class Monster : IPoolable
{
    private SpriteRenderer spriteRenderer;
    private MonsterModel monsterModel;
    private Transform targetTransform;
    private BaseMonsterAttack attackType;

    public void Init()
    {

    }

    public void SetTransform(Transform _transform)
    {
        spriteRenderer = _transform.GetComponent<SpriteRenderer>();

    }

    public void SetWeaponItemModel(MonsterModel _monsterModel)
    {
        monsterModel = _monsterModel;

        //spriteRenderer.sprite = ItemManager.getInstance.GetWeaponItemSprite(monsterModel.itemUid);

        //attackType = ItemManager.getInstance.GetAttackType(monsterModel.attackType);

        //attackType.SetModelInfo(monsterModel);

        //attackType.SetInitPos(weaponTransform);
    }

    public void SetTarget(Transform _target)
    {
        targetTransform = _target;

        //attackType.SetTarget(_target);
    }

    public void Update()
    {
        if (monsterModel == null || targetTransform == null)
        {
            return;
        }

        //attackType.Update();
    }

    

    public void OnEnqueue()
    {
    }

    public void OnDequeue()
    {
    }
}