using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public const int WEAPON_CAPACITY = 6;

    private WeaponItemModel[] equipWeaponModelArr = new WeaponItemModel[6];
    private List<BaseItemModel> attackableItemList = new List<BaseItemModel>();
    private List<BaseItemModel> supportableItemList = new List<BaseItemModel>();

    private List<WeaponItemModel> weaponItemModels = new List<WeaponItemModel>();
    private Dictionary<int, WeaponItemModel> weaponItemDict = new Dictionary<int, WeaponItemModel>();
    private Dictionary<int, Sprite> weaponSpriteDict = new Dictionary<int, Sprite>();
    private Dictionary<EWeaponAttackType, Queue<BaseWeaponAttack>> attackTypeDict = new Dictionary<EWeaponAttackType, Queue<BaseWeaponAttack>>();
    private Sprite[] itemSprites;
    private ObjectPool<Projectile> projectilePool;
    private BaseWeaponAttack[] attackTypeArr = new BaseWeaponAttack[(int)EWeaponAttackType.END];


    public override bool Initialize()
    {
        LoadData();
        InitAttackTypeArr();

        return base.Initialize();
    }

    private void InitAttackTypeArr()
    {
        attackTypeArr[(int)EWeaponAttackType.STING] = new Sting();
        attackTypeArr[(int)EWeaponAttackType.SWING] = new Swing();
        attackTypeArr[(int)EWeaponAttackType.SHOOT] = new Shoot();

        attackTypeDict[EWeaponAttackType.STING] = new Queue<BaseWeaponAttack>();
        attackTypeDict[EWeaponAttackType.SWING] = new Queue<BaseWeaponAttack>();
        attackTypeDict[EWeaponAttackType.SHOOT] = new Queue<BaseWeaponAttack>();
    }

    private void InitProjectilePool()
    {
        projectilePool = PoolManager.getInstance.GetObjectPool<Projectile>();
        projectilePool.Init("Prefabs/Projectile");
    }

    private void LoadData()
    {
        weaponItemModels = TableLoader.LoadFromFile<List<WeaponItemModel>>("Weapon/TestWeapon");
        //Todo WeaponAtlas?
        itemSprites = Resources.LoadAll<Sprite>("Sprites/Props");

        int count = weaponItemModels.Count;

        for (int i = 0; i < count; i++)
        {
            WeaponItemModel itemModel = weaponItemModels[i];

            weaponItemDict.Add(itemModel.itemUid, itemModel);
            weaponSpriteDict.Add(itemModel.itemUid, itemSprites[i]);

        }
    }

    public List<WeaponItemModel> GetAllWeaponModel()
    {
        return weaponItemModels;
    }

    public WeaponItemModel GetWeaponItemModel(int _itemUid)
    {
        weaponItemDict.TryGetValue(_itemUid, out WeaponItemModel itemModel);

        //Debug.Log("Not Exist Item");
        return itemModel;
    }

    public Sprite GetWeaponItemSprite(int _itemUid)
    {
        string itemName = GetWeaponItemModel(_itemUid).itemThumbnail;

        int count = itemSprites.Length;

        for (int i = 0; i < count; i++)
        {
            if(itemName.Equals(itemSprites[i].name))
            {
                return itemSprites[i];
            }
        }

        Debug.Log("Not Exist Sprite");
        return null;

        //weaponSpriteDict.TryGetValue(_itemUid, out Sprite sprite);

        //if(sprite != null)
        //{
        //    return sprite;
        //}

        //Debug.Log("Not Exist Sprite");
        //return null;

    }

    public Sprite GetSpriteToName(string _name)
    {
        int count = itemSprites.Length;

        for (int i = 0; i < count; i++)
        {
            if (_name.Equals(itemSprites[i].name))
            {
                return itemSprites[i];
            }
        }

        Debug.Log("Not Exist Sprite");
        return null;
    }


    public void SetEquipWeaponItem(WeaponItemModel _model, int _slot = 0)
    {
        equipWeaponModelArr[_slot] = _model;
    }

    public WeaponItemModel GetEquipWeaponItemModel(int _slot)
    {
        if(equipWeaponModelArr[_slot] == null)
        {
            return null;
        }

        return equipWeaponModelArr[_slot];
    }

    public Projectile GetProjectile()
    {
        if(projectilePool == null)
        {
            InitProjectilePool();
        }

        return projectilePool.GetObject();
    }

    public void EnqueueProjectile(Projectile _projectile)
    {
        projectilePool.EnqueueObject(_projectile);
    }

    public BaseWeaponAttack GetAttackType(EWeaponAttackType _attackType)
    {
        BaseWeaponAttack weaponAttack = null;

        if (attackTypeDict[_attackType].Count == 0)
        {
            weaponAttack = attackTypeArr[(int)_attackType].DeepCopy();
            attackTypeDict[_attackType].Enqueue(weaponAttack);
        }

        return attackTypeDict[_attackType].Dequeue();
    }

    public void ReleaseWeaponAttackType(EWeaponAttackType _attackType, BaseWeaponAttack _attack)
    {
        attackTypeDict[_attackType].Enqueue(_attack);
    }
}
