using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController
{
    private WeaponItemController[] equipWeaponList;
    private ItemManager itemManager;

    private int weaponCapacity;


    public void Init(Transform _playerTransform)
    {
        itemManager = ItemManager.getInstance;
        weaponCapacity = ItemManager.WEAPON_CAPACITY;
        equipWeaponList = new WeaponItemController[weaponCapacity];

        InitWeaponItem(_playerTransform);
        InitWeapon();

    }

    private void InitWeaponItem(Transform _playerTransform)
    {
        for (int i = 0; i < weaponCapacity; i++)
        {
            equipWeaponList[i] = new WeaponItemController();
            equipWeaponList[i].SetWeaponTransform(_playerTransform.GetChild(i));
        }
    }

    private void InitWeapon()
    {
        for (int i = 0; i < weaponCapacity; i++)
        {
            WeaponItemModel model = itemManager.GetEquipWeaponItemModel(i);
            
            if(model != null)
            {
                equipWeaponList[i].SetWeaponItemModel(model);
            }
        }
    }

    public void Update()
    {
        for(int i = 0; i < weaponCapacity; i++)
        {
            equipWeaponList[i].Update();
        }
    }

    public void EquipWeapon(int _itemSlot ,int _itemUid)
    {
        WeaponItemModel itemModel = itemManager.GetWeaponItemModel(_itemUid);

        equipWeaponList[_itemSlot].SetWeaponItemModel(itemModel);
    }

    public void UnEquipWeapon(int _itemSlot)
    {
        equipWeaponList[_itemSlot].UnEquipWeapon();
    }

    public void SwapWeaponSlot(int _itemSlot,int _chengedItemSlot ,int _itemUid)
    {
        equipWeaponList[_itemSlot].SetWeaponItemModel(itemManager.GetWeaponItemModel(_itemUid));
    }
}