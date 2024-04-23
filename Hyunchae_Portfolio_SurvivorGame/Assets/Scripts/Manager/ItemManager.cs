using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public const int WEAPON_CAPACITY = 6;

    private WeaponItemModel[] equipWeaponModelArr = new WeaponItemModel[6];
    private List<BaseitemModel> attackableItemList = new List<BaseitemModel>();
    private List<BaseitemModel> supportableItemList = new List<BaseitemModel>();

    private List<WeaponItemModel> weaponItemModels = new List<WeaponItemModel>();
    private Dictionary<int, WeaponItemModel> weaponItemDict = new Dictionary<int, WeaponItemModel>();
    private Dictionary<int, Sprite> weaponSpriteDict = new Dictionary<int, Sprite>();
    private Sprite[] itemSprites;


    public override bool Initialize()
    {
        LoadData();
       
        return base.Initialize();
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
        if (weaponItemDict.TryGetValue(_itemUid, out WeaponItemModel itemModel))
        {
            return itemModel;
        }

        Debug.Log("Not Exist Item");
        return null;
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
}