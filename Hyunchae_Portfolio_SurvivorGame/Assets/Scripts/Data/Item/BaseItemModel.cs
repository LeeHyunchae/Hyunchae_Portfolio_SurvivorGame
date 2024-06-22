using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    WEAPON = 0,
    PASSIVE
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



//.. sample

//public class ItemDescTable
//{
//    private List<ItemDesc> descList = null;

//    public void Load()
//    {
//        //..
//    }
//}

//public class ItemStoreTable
//{
//    private List<ItemStoreInfo> storeInfoList = null;

//    public void Load()
//    {
//        //..
//    }

//    public ItemStoreInfo Get(int key)
//    {

//    }
//}

//public class ItemManager : Singleton<ItemManager>
//{

//    private ItemDescTable descTable;
//    private ItemStoreTable storeTablel;

//    public void Load()
//    {
//        descTable.Load();
//        storeTablel.Load();
//    }

//    public ItemStoreInfo GetStoreInfo(int key)
//    {
//        return storeTablel.Get(key);
//    }
//}

//.. 여기서부터 table data
//public class BaseItemModel
//{
//    public int uid;
//}

////.. 3분할 or 1테이블에서 load할때 쪼개\

////.. item desc table
//public class ItemDesc : BaseItemModel
//{
//    public string name;
//    public string desc; //.. 설명
//}

////.. price table
//public class ItemStoreInfo : BaseItemModel
//{
//    public float price;
//}

////.. resource tablle
//public class ItemImage : BaseItemModel
//{
//    public string thumbnailPath; //.. or id
//}

//public class StoreItemData
//{
//    public ItemStoreInfo storeInfo;
//    public ItemDesc desc;
//}
////.. 여기까지

//public class StoreItemObject
//{
//    private StoreItemData data;

//    public void Load(int idx)
//    {
//        //data = StoreManager.Load(idx)
//    }

//    public ItemStoreInfo GetStoreInfo()
//    {
//        return data.storeInfo;
//    }

//    public ItemDesc GetDesc()
//    {
//        return data.desc;
//    }

//}
