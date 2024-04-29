using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    ATTACKABLE = 0,
    SUPPORTABLE
}

public class BaseItemModel
{
    public int itemUid;
    public EItemType itemType;
    public float itemPrice;
    public string itemThumbnail;
    public string bulletImage;
    public string itemName;
    public int[] uniqueAbilityIDArr;

}
