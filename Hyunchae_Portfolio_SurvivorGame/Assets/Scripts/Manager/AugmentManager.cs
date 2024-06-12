using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentManager : Singleton<AugmentManager>
{
    private AugmentData[] augmentDatas;
    private Dictionary<int, AugmentData> augmentDataDict = new Dictionary<int, AugmentData>();

    private List<AugmentData> curAugmentList = new List<AugmentData>();

    public override bool Initialize()
    {
        LoadData();
        return base.Initialize();
    }

    private void LoadData()
    {
        
        List<JsonAugmentData> jsonAugmentDatas = TableLoader.LoadFromFile<List<JsonAugmentData>>("Augment/TestAugment");

        int count = jsonAugmentDatas.Count;

        for (int i = 0; i < count; i++)
        {
            JsonAugmentData jsonData = jsonAugmentDatas[i];

            AugmentData augmentData = new AugmentData();
            augmentData.augmentUid = jsonData.BuildUpID;
            augmentData.augmentTier = jsonData.BuildUpGrade;
            augmentData.augmentName = jsonData.BuildUpName;
            augmentData.augmentImagePath = jsonData.BuildUpImage;
            augmentData.augmentInfo = jsonData.BuildUpContent;
            augmentData.firstAugmentType = (AugmentType)jsonData.BuildUpType;
            augmentData.firstAugmentValue = jsonData.BuildUpVariable;
            augmentData.secondAugmentType = (AugmentType)jsonData.BuildUpType2;
            augmentData.secondAugmentValue = jsonData.BuildUpVariavle2;


            augmentDatas[i] = augmentData;
            augmentDataDict.Add(augmentData.augmentUid, augmentData);
        }
    }

    public AugmentData[] GetAllAugmentData => augmentDatas;

    public AugmentData GetAugmentData(int _augmentUid)
    {
        augmentDataDict.TryGetValue(_augmentUid, out AugmentData data);

#if UNITY_EDITOR

        if (data == null)
        {
            Debug.Log("Not Exist AugmentData");
        }
#endif

        return data;
    }

}
