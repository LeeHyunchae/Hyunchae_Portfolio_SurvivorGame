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
    private ITargetable rotateTargetMonster = null;
    private ObbCollisionObject obbCollision;
    private Transform myTransform;
    private ITargetable[] targetMonsters;
    private Character playerCharacter;
    private DamageData damageData;

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

        spriteRenderer.sprite = itemManager.GetItemSprite(itemModel.itemUid);

        attackType = itemManager.GetAttackType(itemModel.WeaponType);

        attackType.SetModelInfo(itemModel);

        attackType.SetInitPos(myTransform);

        attackType.SetObbCollision(obbCollision);

        obbCollision.RefreshSprite();

        attackType.SetAttackTarget(targetMonsters);

        obbCollision.SetIsCollisionCheck(true);

        obbCollision.OnCollisionAction = OnCollisionMonster;

        SetDamageData();
    }

    public void SetDamageData()
    {
        damageData = new DamageData()
        {
            damage = itemModel.status.damage,
            knockback = itemModel.status.knockback
        };

        attackType.SetDamageData(damageData);
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
        int count = targetMonsters.Length;

        float minDistance = float.MaxValue;

        Vector2 myPos = myTransform.position;

        bool isAllMonsterDead = true;

        for (int i = 0; i < count; i ++)
        {
            ITargetable target = targetMonsters[i];

            if(target.GetIsDead())
            {
                continue;
            }

            isAllMonsterDead = false;

            Vector2 targetPos = target.GetTransform().position;

            float distance = Vector2.Distance(targetPos, myPos);

            if (distance < minDistance)
            {
                minDistance = distance;
                rotateTargetMonster = target;
            }
        }

        if (rotateTargetMonster == null || isAllMonsterDead)
        {
            rotateTargetMonster = null;
            attackType.RemoveTarget();
            spriteRenderer.flipY = false;

            return;
        }

        if (rotateTargetMonster.GetTransform().position.x < myPos.x)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }

        attackType.SetRotateTarget(rotateTargetMonster.GetTransform());


        //LinkedList<MonsterController> monsters = monsterManager.GetAllAliveMonsters;

        //float minDistance = float.MaxValue;

        //Vector2 myPos = myTransform.position;

        //foreach(MonsterController monster in monsters)
        //{
        //    if(!monster.GetMonsterTransform.gameObject.activeSelf)
        //    {
        //        continue;
        //    }

        //    Vector2 monsterPos = monster.GetMonsterTransform.position;

        //    float distance = Vector2.Distance(monsterPos, myPos);

        //    if(distance < minDistance)
        //    {
        //        minDistance = distance;
        //        rotateTargetMonster = monster;
        //    }
        //}

        //if(rotateTargetMonster == null || monsters.Count == 0)
        //{
        //    rotateTargetMonster = null;
        //    attackType.RemoveTarget();
        //    spriteRenderer.flipY = false;

        //    return;
        //}

        //if(rotateTargetMonster.GetMonsterTransform.position.x < myPos.x)
        //{
        //    spriteRenderer.flipY = true;
        //}else
        //{
        //    spriteRenderer.flipY = false;
        //}

        //attackType.SetRotateTarget(rotateTargetMonster.GetMonsterTransform);

    }

    public void OnCollisionMonster(ITargetable _target)
    {
        _target.OnDamaged(damageData);
    }
}
