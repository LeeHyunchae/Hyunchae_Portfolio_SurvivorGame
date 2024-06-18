using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private WeaponItemModel itemModel;
    private BaseWeaponAttack attackType;
    private MonsterManager monsterManager;
    private ItemManager itemManager;
    private MonsterController rotateTargetMonster = null;
    private ObbCollisionObject obbCollision;
    private Transform myTransform;
    private ITargetable[] targetMonsters;
    private Character playerCharacter;

    public void Init()
    {
        monsterManager = MonsterManager.getInstance;
        itemManager = ItemManager.getInstance;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        obbCollision = gameObject.GetComponent<ObbCollisionObject>();

        myTransform = gameObject.GetComponent<Transform>();

        playerCharacter = CharacterManager.getInstance.GetPlayerCharacter;
    }
    public void SetWeaponItemModel(WeaponItemModel _itemModel)
    {
        itemModel = _itemModel;

        spriteRenderer.sprite = itemManager.GetSpriteToName(itemModel.itemThumbnail);

        attackType = itemManager.GetAttackType(itemModel.WeaponType);

        attackType.SetModelInfo(itemModel);

        attackType.SetInitPos(myTransform);

        attackType.SetObb(obbCollision);

        obbCollision.RefreshSprite();

        attackType.SetAttackTarget(targetMonsters);

        obbCollision.SetIsCollisionCheck(true);
    }

    public void SetTargetMonsters(ITargetable[] _targetMonsters)
    {
        targetMonsters = _targetMonsters;
        obbCollision.SetTarget(targetMonsters);
    }

    private void Update()
    {
        if (itemModel == null && rotateTargetMonster == null)
        {
            return;
        }
        FindTarget();

        attackType.Update();
    }

    public void UnEquipWeapon()
    {
        if (itemModel != null)
        {
            itemManager.ReleaseWeaponAttackType(itemModel.WeaponType, attackType);
        }
        attackType = null;
        itemModel = null;
        spriteRenderer.sprite = null;

        obbCollision.SetIsCollisionCheck(false);
    }

    private void FindTarget()
    {
        LinkedList<MonsterController> monsters = monsterManager.GetAllAliveMonsters;

        float minDistance = float.MaxValue;

        Vector2 myPos = myTransform.position;

        foreach(MonsterController monster in monsters)
        {
            if(!monster.GetMonsterTransform.gameObject.activeSelf)
            {
                continue;
            }

            Vector2 monsterPos = monster.GetMonsterTransform.position;

            float distance = Vector2.Distance(monsterPos, myPos);

            if(distance < minDistance)
            {
                minDistance = distance;
                rotateTargetMonster = monster;
            }
        }

        if(rotateTargetMonster == null || monsters.Count == 0)
        {
            rotateTargetMonster = null;
            attackType.RemoveTarget();
            spriteRenderer.flipY = false;

            return;
        }

        if(rotateTargetMonster.GetMonsterTransform.position.x < myPos.x)
        {
            spriteRenderer.flipY = true;
        }else
        {
            spriteRenderer.flipY = false;
        }

        attackType.SetRotateTarget(rotateTargetMonster.GetMonsterTransform);

    }

}
