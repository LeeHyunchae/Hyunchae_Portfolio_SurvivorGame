using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : Singleton<ItemManager>
{
    public const int WEAPON_CAPACITY = 6;
    private const int ITEM_TIER_COUNT = 4;

    private WeaponItemModel[] equipWeaponModelArr = new WeaponItemModel[6];
    private List<BaseItemModel> attackableItemList = new List<BaseItemModel>();
    private List<BaseItemModel> supportableItemList = new List<BaseItemModel>();

    private List<WeaponItemModel> weaponItemModels = new List<WeaponItemModel>();
    private Dictionary<int, WeaponItemModel> weaponItemUidDict = new Dictionary<int, WeaponItemModel>();
    //private Dictionary<int, List<WeaponItemModel>> weaponItemGroupDict = new Dictionary<int, List<WeaponItemModel>>();
    //private Dictionary<int, Sprite> weaponSpriteDict = new Dictionary<int, Sprite>();
    private List<Sprite> itemSprites = new List<Sprite>();
    private ObjectPool<Projectile> projectilePool;

    private BaseWeaponAttack[] attackTypeArr = new BaseWeaponAttack[(int)EWeaponType.END];
    private Dictionary<EWeaponType, Queue<BaseWeaponAttack>> attackTypeDict = new Dictionary<EWeaponType, Queue<BaseWeaponAttack>>();
    private int pieceCount = 0;
    public Action OnRefreshEquipWeaponList;

    public int GetWeaponCapacity => WEAPON_CAPACITY;

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

        attackTypeDict[EWeaponType.STING] = new Queue<BaseWeaponAttack>();
        attackTypeDict[EWeaponType.SWING] = new Queue<BaseWeaponAttack>();
        attackTypeDict[EWeaponType.SHOOT] = new Queue<BaseWeaponAttack>();
    }

    private void InitProjectilePool()
    {
        projectilePool = PoolManager.getInstance.GetObjectPool<Projectile>();
        projectilePool.Init("Prefabs/Projectile");
    }

    private void LoadData()
    {
        LoadWeaponSprite();

        List<JsonWeaponData> jsonWeaponDatas = TableLoader.LoadFromFile<List<JsonWeaponData>>("Weapon/TestWeapon");

        int count = jsonWeaponDatas.Count;

        for (int i = 0; i < count; i++)
        {
            JsonWeaponData weaponData = jsonWeaponDatas[i];

            WeaponItemModel itemModel = new WeaponItemModel
            {
                itemUid = weaponData.WeaponID,
                itemType = EItemType.ATTACKABLE,
                itemPrice = 0,
                itemThumbnail = weaponData.ItemImage,
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

            weaponItemUidDict.Add(itemModel.itemUid, itemModel);
            weaponItemModels.Add(itemModel);
        }

    }

    private void LoadWeaponSprite()
    {

        for (int i = 0; i < ITEM_TIER_COUNT; i++)
        {
            int idx = i + 1;

            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Tier" + idx + "_Props");

            int spritesCount = sprites.Length;

            for (int j = 0; j < spritesCount; j++)
            {
                itemSprites.Add(sprites[j]);
            }
        }


    }

    public List<WeaponItemModel> GetAllWeaponModel()
    {
        return weaponItemModels;
    }

    public List<BaseItemModel> GetRandomItemList()
    {
        List<BaseItemModel> randomItemList = new List<BaseItemModel>();

        //int randomWeaponItemCount = Random.Range(0, 5);
        int randomWeaponItemCount = 4;

        int weaponItemCount = weaponItemModels.Count;

        for(int i = 0; i < randomWeaponItemCount; i++)
        {
            int randomNum = Random.Range(0, weaponItemCount);

            randomItemList.Add(weaponItemModels[randomNum]);
        }

        return randomItemList;
    }

    public WeaponItemModel GetWeaponItemModel(int _itemUid)
    {
        weaponItemUidDict.TryGetValue(_itemUid, out WeaponItemModel itemModel);

        //Debug.Log("Not Exist Item");
        return itemModel;
    }

    public Sprite GetWeaponItemSprite(int _itemUid)
    {
        string itemName = GetWeaponItemModel(_itemUid).itemThumbnail;

        int count = itemSprites.Count;

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
        int count = itemSprites.Count;

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


    private void SetEquipWeaponItem(WeaponItemModel _model = null, int _slot = 0)
    {
        equipWeaponModelArr[_slot] = _model;

        OnRefreshEquipWeaponList?.Invoke();
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

    public int GetPieceCount()
    {
        return pieceCount;
    }

    public void AddEquipWeaponItem(int _itemUid)
    {
        for(int i = 0; i <WEAPON_CAPACITY; i++)
        {
            if(equipWeaponModelArr[i] != null)
            {
                continue;
            }

            SetEquipWeaponItem(GetWeaponItemModel(_itemUid), i);
            break;
        }
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
            WeaponItemModel model = equipWeaponModelArr[i];

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
        WeaponItemModel combineTarget = equipWeaponModelArr[_slotNum];

        int count = WEAPON_CAPACITY;

        for(int i = 0; i <count; i++)
        {
            WeaponItemModel model = equipWeaponModelArr[i];

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
        WeaponItemModel combineTarget = equipWeaponModelArr[_slotNum];
        int combineTargetGroup = combineTarget.weaponGroup;
        int combineTargetTier = combineTarget.weaponTier + 1;

        if(combineTargetTier > 3)
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
