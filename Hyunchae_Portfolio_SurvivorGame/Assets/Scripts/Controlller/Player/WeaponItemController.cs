using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemController
{
    private SpriteRenderer spriteRenderer;
    private WeaponItemModel itemModel;
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

        attackType.SetInitPos(weaponTransform);
    }

    public void Update()
    {
        if (itemModel == null)
        {
            return;
        }
        FindTarget();

        attackType.Update();
    }

    public void UnEquipWeapon()
    {
        ItemManager.getInstance.ReleaseWeaponAttackType(itemModel.attackType, attackType);
        attackType = null;
        itemModel = null;
        spriteRenderer.sprite = null;
    }

    private void FindTarget()
    {
        LinkedList<MonsterController> monsters = MonsterManager.getInstance.GetAllAliveMonsters;

        float minDistance = float.MaxValue;

        Vector2 myPos = weaponTransform.position;

        MonsterController targetMonster = null;

        foreach(MonsterController monster in monsters)
        {
            Vector2 monsterPos = monster.GetMonsterTransform.position;

            float distance = Vector2.Distance(monsterPos, myPos);

            if(distance < minDistance)
            {
                minDistance = distance;
                targetMonster = monster;
            }
        }

        attackType.SetTarget(targetMonster.GetMonsterTransform);

    }
}
