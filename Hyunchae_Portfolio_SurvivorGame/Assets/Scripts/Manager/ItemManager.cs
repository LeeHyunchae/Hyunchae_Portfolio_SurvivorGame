using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : Singleton<ItemManager>
{
    public const int WEAPON_CAPACITY = 6;
    private const int ITEM_TIER_COUNT = 4;

    private BaseItemModel[] equipWeaponModelArr = new BaseItemModel[WEAPON_CAPACITY];
    private List<BaseItemModel> equipPassiveModelList = new List<BaseItemModel>();

    private List<WeaponItemModel> weaponItemModels = new List<WeaponItemModel>();
    private List<PassiveItemModel> passiveItemModels = new List<PassiveItemModel>();

    private Dictionary<int, BaseItemModel> itemDict = new Dictionary<int, BaseItemModel>();
    private Dictionary<int, Sprite> itemSpritesDict = new Dictionary<int, Sprite>();

    private ObjectPool<Projectile> projectilePool;
    private List<Sprite> projectileSprites = new List<Sprite>();

    private BaseWeaponAttack[] attackTypeArr = new BaseWeaponAttack[(int)EWeaponType.END];
    private Dictionary<EWeaponType, Queue<BaseWeaponAttack>> attackTypeDict = new Dictionary<EWeaponType, Queue<BaseWeaponAttack>>();
    public Action OnRefreshEquipWeaponList;
    public Action OnRefreshEquipPassiveList;

    public int GetWeaponCapacity => WEAPON_CAPACITY;
    public List<BaseItemModel> GetAllEquipPassiveItemModelList => equipPassiveModelList;

    public override bool Initialize()
    {
        LoadData();
        InitAttackTypeArr();

        return base.Initialize();
    }

    private void InitAttackTypeArr()
    {
        attackTypeArr[(int)EWeaponType.STING] = new Sting();
        attackTypeArr[(int)EWeaponType.SWING] = new Swing();
        attackTypeArr[(int)EWeaponType.SHOOT] = new Shoot();

        int count = (int)EWeaponType.END;

        for(int i = 0; i < count; i++)
        {
            attackTypeDict[(EWeaponType)i] = new Queue<BaseWeaponAttack>();
        }

    }

    private void InitProjectilePool()
    {
        projectilePool = PoolManager.getInstance.GetObjectPool<Projectile>();
        projectilePool.Init("Prefabs/Projectile");
    }

    private void LoadData()
    {
        LoadWeaponData();

        LoadPassiveItemData();

        LoadItemSprite();
    }

    private void LoadWeaponData()
    {
        List<JsonWeaponData> jsonWeaponDatas = TableLoader.LoadFromFile<List<JsonWeaponData>>("Weapon/TestWeapon");

        int count = jsonWeaponDatas.Count;

        for (int i = 0; i < count; i++)
        {
            JsonWeaponData weaponData = jsonWeaponDatas[i];

            WeaponItemModel itemModel = new WeaponItemModel
            {
                itemUid = weaponData.WeaponID,
                itemPrice = weaponData.ItemPrice,
                itemThumbnail = weaponData.ItemImage,
                itemType = EItemType.WEAPON,
                bulletImage = weaponData.BulletName,
                itemName = weaponData.ItemName,
                WeaponType = weaponData.WeaponType,
                weaponGroup = weaponData.WeaponGroup,
                weaponTier = weaponData.WeaponTier,
                weaponSenergy = weaponData.WeaponSynergy,
                status = new WeaponStatus
                {
                    attackType = weaponData.WeaponAttackType,
                    damage = weaponData.WeaponDamage,
                    criticalChance = weaponData.WeaponCritical,
                    flatDamage = weaponData.WeaponTypeDamage,
                    range = weaponData.WeaponRange,
                    speed = weaponData.WeaponSpeed,
                    cooldown = weaponData.WeaponCoolDown,
                    knockback = weaponData.WeaponKnockback
                }
            };

            itemDict.Add(itemModel.itemUid, itemModel);
            weaponItemModels.Add(itemModel);
        }
    }

    private void LoadPassiveItemData()
    {
        List<JsonPassiveItemModel> jsonEquipItemDatas = TableLoader.LoadFromFile<List<JsonPassiveItemModel>>("PassiveItem/TestPassive");

        int count = jsonEquipItemDatas.Count;

        for (int i = 0; i < count; i++)
        {
            JsonPassiveItemModel equipItemData = jsonEquipItemDatas[i];

            PassiveItemModel itemModel = new PassiveItemModel
            {
                itemUid = equipItemData.ItemID,
                itemTier = equipItemData.ItemTier,
                itemPrice = equipItemData.ItemPrice,
                itemThumbnail = equipItemData.ItemImage,
                itemType = EItemType.PASSIVE,
                bulletImage = equipItemData.BulletName,
                itemName = equipItemData.ItemName,
                itemInfo = equipItemData.ItemContent,
                status_Variances = equipItemData.ItemStatusEffect

            };

            itemDict.Add(itemModel.itemUid, itemModel);
            passiveItemModels.Add(itemModel);
        }
    }

    private void LoadItemSprite()
    {
        List<Sprite> weaponSpriteList = new List<Sprite>();

        for (int i = 0; i < ITEM_TIER_COUNT; i++)
        {
            int idx = i + 1;

            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Tier" + idx + "_Props");

            int weaponSpritesCount = sprites.Length;

            for (int j = 0; j < weaponSpritesCount; j++)
            {
                weaponSpriteList.Add(sprites[j]);

                if(sprites[j].name == "Tier1_Props_9" ||
                    sprites[j].name == "Tier2_Props_12")
                {
                    projectileSprites.Add(sprites[j]);
                }
            }


        }

        List<Sprite> passiveSpriteList = new List<Sprite>();
        Sprite[] passiveSprites = Resources.LoadAll<Sprite>("Sprites/UI");

        int spritesCount = passiveSprites.Length;

        for (int j = 0; j < spritesCount; j++)
        {
            passiveSpriteList.Add(passiveSprites[j]);
        }

        int weaponCount = weaponItemModels.Count;

        for(int i = 0; i < weaponCount; i++)
        {
            for(int j = 0; j < weaponSpriteList.Count; j++)
            {
                if(weaponItemModels[i].itemThumbnail == weaponSpriteList[j].name)
                {
                    itemSpritesDict.Add(weaponItemModels[i].itemUid, weaponSpriteList[j]);
                }
            }

            
        }

        int passiveCount = passiveItemModels.Count;

        for (int i = 0; i < passiveCount; i++)
        {
            for (int j = 0; j < passiveSpriteList.Count; j++)
            {
                if (passiveItemModels[i].itemThumbnail == passiveSpriteList[j].name)
                {
                    itemSpritesDict.Add(passiveItemModels[i].itemUid, passiveSpriteList[j]);
                }
            }
        }
    }

    public List<WeaponItemModel> GetAllWeaponModel()
    {
        return weaponItemModels;
    }

    public List<PassiveItemModel> GetAllPassiveItemModel()
    {
        return passiveItemModels;
    }

    public List<BaseItemModel> GetRandomItemList(int _count = 4)
    {
        List<BaseItemModel> randomItemList = new List<BaseItemModel>();

        int weaponItemCount = weaponItemModels.Count;
        int passiveItemCount = passiveItemModels.Count;

        for(int i = 0; i < _count; i++)
        {
            BaseItemModel itemModel;

            bool isWeapon = (Random.value > 0.5f);
            int randomNum;

            if (isWeapon)
            {
                randomNum = Random.Range(0, weaponItemCount);
                itemModel = weaponItemModels[randomNum];
            }
            else
            {
                randomNum = Random.Range(0, passiveItemCount);
                itemModel = passiveItemModels[randomNum];
            }


            randomItemList.Add(itemModel);
        }

        return randomItemList;
    }

    public BaseItemModel GetItemModel(int _itemUid)
    {
        itemDict.TryGetValue(_itemUid, out BaseItemModel itemModel);

        //Debug.Log("Not Exist Item");
        return itemModel;
    }

    public Sprite GetItemSprite(int _itemUid)
    {
        itemSpritesDict.TryGetValue(_itemUid, out Sprite sprite);

        if (sprite != null)
        {
            return sprite;
        }

        Debug.Log("Not Exist Item Sprite");
        return null;

    }

    public Sprite GetProjectileSprite(string _spriteName)
    {
        int count = projectileSprites.Count;

        for(int i = 0; i< count; i ++)
        {
            if(projectileSprites[i].name == _spriteName)
            {
                return projectileSprites[i];
            }
        }

        Debug.Log("Not Exist Projectile Sprite");
        return null;
    }

    private void SetEquipWeaponItem(BaseItemModel _model = null, int _slot = 0)
    {
        equipWeaponModelArr[_slot] = _model;

        OnRefreshEquipWeaponList?.Invoke();
    }

    public BaseItemModel GetEquipWeaponItemModel(int _slot)
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

    public BaseWeaponAttack GetAttackType(EWeaponType _attackType)
    {

        if (attackTypeDict[_attackType].Count == 0)
        {
            BaseWeaponAttack weaponAttack = attackTypeArr[(int)_attackType].DeepCopy();
            attackTypeDict[_attackType].Enqueue(weaponAttack);
        }

        return attackTypeDict[_attackType].Dequeue();
    }

    public void ReleaseWeaponAttackType(EWeaponType _attackType, BaseWeaponAttack _attack)
    {
        attackTypeDict[_attackType].Enqueue(_attack);
    }

    public bool OnBuyItem(int _itemUid)
    {
        bool isSuccess = true;

        if (GetItemModel(_itemUid).itemType == EItemType.WEAPON)
        {
            isSuccess = AddEquipWeaponItem(_itemUid);
        }
        else
        {
            AddEquipPassiveItem(_itemUid);
        }

        return isSuccess;
    }

    private bool AddEquipWeaponItem(int _itemUid)
    {
        bool isSucess = false;

        for(int i = 0; i <WEAPON_CAPACITY; i++)
        {
            if(equipWeaponModelArr[i] != null)
            {
                continue;
            }

            isSucess = true;
            SetEquipWeaponItem(GetItemModel(_itemUid), i);
            break;
        }

        return isSucess;
    }

    private void AddEquipPassiveItem(int _itemUid)
    {
        BaseItemModel itemModel = GetItemModel(_itemUid);

        equipPassiveModelList.Add(itemModel);

        OnRefreshEquipPassiveList?.Invoke();
    }

    public void SellWeaponItem(int _slotNum)
    {
        SetEquipWeaponItem(null, _slotNum);

        //Todo Piece += itemPice ,,, GlobalData
    }

    public bool CheckCombineItemExistence(WeaponItemModel _model)
    {
        WeaponItemModel compareTarget = _model;

        int count = WEAPON_CAPACITY;

        int duplicationCount = 0;

        for (int i = 0; i < count; i++)
        {
            BaseItemModel model = equipWeaponModelArr[i];

            if (model == null)
            {
                continue;
            }
            else
            {
                if (compareTarget == model)
                {
                    duplicationCount++;
                }
            }
        }

        if (duplicationCount >= 2)
        {
            Debug.Log("isDuplicated");
            return true;
        }

        Debug.Log("NotDuplicated");
        return false;
    }

    public void CombineWeaponItem(int _slotNum)
    {
        BaseItemModel combineTarget = equipWeaponModelArr[_slotNum];

        int count = WEAPON_CAPACITY;

        for(int i = 0; i <count; i++)
        {
            BaseItemModel model = equipWeaponModelArr[i];

            if(i == _slotNum)
            {
                continue;
            }

            if(combineTarget == model)
            {
                if(UpgradeWeaponItem(_slotNum))
                {
                    SetEquipWeaponItem(null, i);

                }
                break;
            }
        }
    }

    public bool UpgradeWeaponItem(int _slotNum)
    {
        WeaponItemModel combineTarget = equipWeaponModelArr[_slotNum] as WeaponItemModel;
        int combineTargetGroup = combineTarget.weaponGroup;
        int combineTargetTier = combineTarget.weaponTier + 1;

        if(combineTargetTier > ITEM_TIER_COUNT -1)
        {
            Debug.Log("This Item Is Last Tier");
            return false;
        }

        int count = weaponItemModels.Count;

        for (int i = 0; i < count; i++)
        {
            WeaponItemModel model = weaponItemModels[i];

            if(combineTargetGroup == model.weaponGroup && combineTargetTier == model.weaponTier)
            {
                SetEquipWeaponItem(model, _slotNum);
                return true;
            }
        }

        Debug.Log("Not Exist Next Tier Item");
        return false;
    }
}
