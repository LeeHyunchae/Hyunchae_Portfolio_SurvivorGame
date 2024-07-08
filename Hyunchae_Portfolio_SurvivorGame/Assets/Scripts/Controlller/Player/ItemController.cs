using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController
{
    private WeaponItemController[] equipWeaponList;
    private ItemManager itemManager;

    private int weaponCapacity;

    private ITargetable[] targetMonsters;

    public void Init(Transform _playerTransform)
    {
        itemManager = ItemManager.getInstance;
        weaponCapacity = ItemManager.WEAPON_CAPACITY;
        equipWeaponList = new WeaponItemController[weaponCapacity];

        itemManager.OnRefreshEquipWeaponList += OnRefreshEquipWeaponList;

        InitWeaponItem(_playerTransform);
        InitTargetMonsters();
        OnRefreshEquipWeaponList();

    }

    private void InitWeaponItem(Transform _playerTransform)
    {
        for (int i = 0; i < weaponCapacity; i++)
        {
            equipWeaponList[i] = _playerTransform.GetChild(i).GetComponent<WeaponItemController>();
            equipWeaponList[i].Init();
        }
    }

    private void InitTargetMonsters()
    {
        for (int i = 0; i < weaponCapacity; i++)
        {
            equipWeaponList[i].SetTargetMonsters(targetMonsters);
        }
    }

    public void SetTargetMonsters(ITargetable[] _targetMonsters)
    {
        targetMonsters = _targetMonsters;
    }

    public void EquipWeapon(int _itemSlot ,int _itemUid)
    {
        WeaponItemModel itemModel = itemManager.GetItemModel(_itemUid) as WeaponItemModel;

        equipWeaponList[_itemSlot].SetWeaponItemModel(itemModel);
    }

    public void UnEquipWeapon(int _itemSlot)
    {
        equipWeaponList[_itemSlot].UnEquipWeapon();
    }

    public void SwapWeaponSlot(int _itemSlot,int _chengedItemSlot ,int _itemUid)
    {
        equipWeaponList[_itemSlot].SetWeaponItemModel(itemManager.GetItemModel(_itemUid) as WeaponItemModel);
    }

    private void OnRefreshEquipWeaponList()
    {
        for (int i = 0; i < weaponCapacity; i++)
        {
            WeaponItemModel model = itemManager.GetEquipWeaponItemModel(i) as WeaponItemModel;

            if(equipWeaponList[i] == null)
            {
                break;
            }

            if (model != null)
            {
                equipWeaponList[i].SetWeaponItemModel(model);
            }
            else
            {
                equipWeaponList[i].UnEquipWeapon();
            }
        }
    }
}