using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    ATTACKABLE = 0,
    SUPPORTABLE
}

public class Baseitem
{
    public int itemUid;
    public EItemType itemType;
    public float itemPrice;
    public string itemThumbnail;
    public string itemName;
    public int[] unique_Ability_IDArr;

    public void Update()
    {
        Fire();
    }

    public virtual void Fire() 
    {
        if(itemType == EItemType.SUPPORTABLE)
        {
            return;
        }
    }
}
