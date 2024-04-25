using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController
{
    private WeaponItem[] equipWeaponList;
    private ItemManager itemManager;

    private int weaponCapacity;


    public void Init(Transform _playerTransform)
    {
        itemManager = ItemManager.getInstance;
        weaponCapacity = ItemManager.WEAPON_CAPACITY;
        equipWeaponList = new WeaponItem[weaponCapacity];

        InitWeaponItem(_playerTransform);
        InitWeapon();

    }

    public void SetTempEnemy(Transform _targetTransform)
    {
        // Todo Delete!!!!!!!!!!
        for (int i = 0; i < weaponCapacity; i++)
        {
            equipWeaponList[i].SetTarget(_targetTransform);
        }
    }

    private void InitWeaponItem(Transform _playerTransform)
    {
        for (int i = 0; i < weaponCapacity; i++)
        {
            equipWeaponList[i] = new WeaponItem();
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



//private void InitWeaponPosition()
//{
//    Vector3 playerPosition = target.position;

//    float radius = 1.0f;
//    Vector3[] hexagonPoints = new Vector3[6];

//    //Todo Refactoring

//    hexagonPoints[0] = new Vector3(playerPosition.x + radius * 0.5f, playerPosition.y + Mathf.Sqrt(3) * radius * 0.5f, playerPosition.z);
//    hexagonPoints[1] = new Vector3(playerPosition.x - radius * 0.5f, playerPosition.y + Mathf.Sqrt(3) * radius * 0.5f, playerPosition.z);
//    hexagonPoints[2] = new Vector3(playerPosition.x + radius, playerPosition.y, playerPosition.z);
//    hexagonPoints[3] = new Vector3(playerPosition.x - radius, playerPosition.y, playerPosition.z);
//    hexagonPoints[4] = new Vector3(playerPosition.x + radius * 0.5f, playerPosition.y - Mathf.Sqrt(3) * radius * 0.5f, playerPosition.z);
//    hexagonPoints[5] = new Vector3(playerPosition.x - radius * 0.5f, playerPosition.y - Mathf.Sqrt(3) * radius * 0.5f, playerPosition.z);

//    for (int i = 0; i < 6; i++)
//    {
//        equipWeaponList[i].GetTransform.position = hexagonPoints[i];
//        equipWeaponList[i].GetTransform.SetParent(target);
//    }
//}