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

    public void Init(GameObject[] _weaponObjArr)
    {
        itemManager = ItemManager.getInstance;
        weaponCapacity = ItemManager.WEAPON_CAPACITY;
        equipWeaponList = new WeaponItem[weaponCapacity];
        
        InitWeaponItemObject(_weaponObjArr);
        InitWeapon();

    }

    public void SetTarget(Transform _transform)
    {
        target = _transform;

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

    private void InitWeaponItemObject(GameObject[] _weaponObjArr)
    {
        for (int i = 0; i < weaponCapacity; i++)
        {
            equipWeaponList[i] = new WeaponItem();
            equipWeaponList[i].SetWeaponObject(_weaponObjArr[i]);
        }

        InitWeaponPosition();
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


    private void InitWeaponPosition()
    {
        Vector3 playerPosition = target.position;

        // 육각형의 각 점의 각도 (60도씩 회전)
        float angleIncrement = 360f / 6;
        // 육각형의 반지름
        float radius = 1.0f;
        // 육각형의 각 점을 저장할 배열
        Vector3[] hexagonPoints = new Vector3[6];

        //// 각 점의 좌표를 계산하여 배열에 저장
        //for (int i = 0; i < 6; i++)
        //{
        //    float angle = i * angleIncrement; // 현재 점의 각도
        //    float x = playerPosition.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
        //    float z = playerPosition.z + radius * Mathf.Sin(Mathf.Deg2Rad * angle);
        //    hexagonPoints[i] = new Vector3(x, z, 0);

        //    equipWeaponList[i].GetTransform.position = hexagonPoints[i];

        //    equipWeaponList[i].GetTransform.SetParent(target);
        //}

        //float startAngle = 30f;

        //// 각 점의 좌표를 계산하여 배열에 저장
        //for (int i = 0; i < 6; i++)
        //{
        //    float angle = startAngle + i * angleIncrement; // 현재 점의 각도
        //    float x = playerPosition.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
        //    float z = playerPosition.z + radius * Mathf.Sin(Mathf.Deg2Rad * angle);
        //    hexagonPoints[i] = new Vector3(x, z, 0);

        //    // 무기 배치
        //    int weaponIndex = (i + 1) % 6; // 1부터 시작하도록 인덱스를 조정
        //    equipWeaponList[weaponIndex].GetTransform.position = hexagonPoints[i];
        //    equipWeaponList[weaponIndex].GetTransform.SetParent(target);
        //}

        //Todo Refactoring

        hexagonPoints[0] = new Vector3(playerPosition.x + radius * 0.5f, playerPosition.y + Mathf.Sqrt(3) * radius * 0.5f, playerPosition.z);
        hexagonPoints[1] = new Vector3(playerPosition.x - radius * 0.5f, playerPosition.y + Mathf.Sqrt(3) * radius * 0.5f, playerPosition.z);
        hexagonPoints[2] = new Vector3(playerPosition.x + radius, playerPosition.y, playerPosition.z);
        hexagonPoints[3] = new Vector3(playerPosition.x - radius, playerPosition.y, playerPosition.z);
        hexagonPoints[4] = new Vector3(playerPosition.x + radius * 0.5f, playerPosition.y - Mathf.Sqrt(3) * radius * 0.5f, playerPosition.z);
        hexagonPoints[5] = new Vector3(playerPosition.x - radius * 0.5f, playerPosition.y - Mathf.Sqrt(3) * radius * 0.5f, playerPosition.z);

        // 각 무기를 육각형의 각 점에 배치
        for (int i = 0; i < 6; i++)
        {
            equipWeaponList[i].GetTransform.position = hexagonPoints[i];
            equipWeaponList[i].GetTransform.SetParent(target);
        }
    }

    public void GetWeapon(int _itemUid)
    {
        WeaponItemModel itemModel = itemManager.GetWeaponItemModel(_itemUid);

    }

    public void SwapWeapon(int _itemSlot, int _itemUid)
    {
        equipWeaponList[_itemSlot].SetWeaponItemModel(itemManager.GetWeaponItemModel(_itemUid));
    }
}
