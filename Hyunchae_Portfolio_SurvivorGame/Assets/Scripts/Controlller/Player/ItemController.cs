using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController
{
    private WeaponItem[] equipWeaponList;
    private ItemManager itemManager;

    private Transform target;
    private int weaponCapacity;

    private Transform tempEnemy;

    public void Init(Transform _targetTransform)
    {
        itemManager = ItemManager.getInstance;
        weaponCapacity = ItemManager.WEAPON_CAPACITY;
        equipWeaponList = new WeaponItem[weaponCapacity];

        SetTarget(_targetTransform);
        InitWeaponItem();
        InitWeapon();

    }

    public void SetTarget(Transform _target)
    {
        target = _target;

    }

    public void SetTempEnemy(Transform _transform)
    {
        // Todo Delete!!!!!!!!!!
        tempEnemy = _transform;


        for (int i = 0; i < weaponCapacity; i++)
        {
            equipWeaponList[i].SetTarget(tempEnemy);
        }
    }

    private void InitWeaponItem()
    {
        for (int i = 0; i < weaponCapacity; i++)
        {
            equipWeaponList[i] = new WeaponItem();
            equipWeaponList[i].SetWeaponTransform(target.GetChild(i));
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

    public void GetWeapon(int _itemUid)
    {
        WeaponItemModel itemModel = itemManager.GetWeaponItemModel(_itemUid);

    }

    public void SwapWeapon(int _itemSlot, int _itemUid)
    {
        equipWeaponList[_itemSlot].SetWeaponItemModel(itemManager.GetWeaponItemModel(_itemUid));
    }
}
