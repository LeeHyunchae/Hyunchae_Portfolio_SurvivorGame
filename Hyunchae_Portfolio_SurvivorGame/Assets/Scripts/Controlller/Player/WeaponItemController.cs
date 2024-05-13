using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemController
{
    private SpriteRenderer spriteRenderer;
    private WeaponItemModel itemModel;
    private Transform weaponTransform;
    private BaseWeaponAttack attackType;
    private MonsterManager monsterManager;
    private ItemManager itemManager;
    private MonsterController targetMonster = null;

    public void Init()
    {
        monsterManager = MonsterManager.getInstance;
        itemManager = ItemManager.getInstance;
    }
    public void SetWeaponTransform(Transform _weaponTransform)
    {
        spriteRenderer = _weaponTransform.GetComponent<SpriteRenderer>();

        weaponTransform = _weaponTransform;
    }

    public void SetWeaponItemModel(WeaponItemModel _itemModel)
    {
        itemModel = _itemModel;

        spriteRenderer.sprite = itemManager.GetWeaponItemSprite(itemModel.itemUid);

        attackType = itemManager.GetAttackType(itemModel.attackType);

        attackType.SetModelInfo(itemModel);

        attackType.SetInitPos(weaponTransform);
    }

    public void Update()
    {
        if (itemModel == null || targetMonster == null)
        {
            return;
        }
        FindTarget();

        attackType.Update();
    }

    public void UnEquipWeapon()
    {
        itemManager.ReleaseWeaponAttackType(itemModel.attackType, attackType);
        attackType = null;
        itemModel = null;
        spriteRenderer.sprite = null;
    }

    private void FindTarget()
    {
        LinkedList<MonsterController> monsters = monsterManager.GetAllAliveMonsters;

        float minDistance = float.MaxValue;

        Vector2 myPos = weaponTransform.position;

        foreach(MonsterController monster in monsters)
        {
            if(!monster.GetMonsterTransform.gameObject.activeSelf)
            {
                Debug.Log("continue");
                continue;
            }

            Vector2 monsterPos = monster.GetMonsterTransform.position;

            float distance = Vector2.Distance(monsterPos, myPos);

            if(distance < minDistance)
            {
                minDistance = distance;
                targetMonster = monster;
            }
        }

        if(targetMonster == null)
        {
            return;
        }

        //Temp Code.. Need WeaponSprite
        if(targetMonster.GetMonsterTransform.position.x < myPos.x)
        {
            spriteRenderer.flipY = true;
        }else
        {
            spriteRenderer.flipY = false;
        }

        attackType.SetTarget(targetMonster.GetMonsterTransform);

    }
}
