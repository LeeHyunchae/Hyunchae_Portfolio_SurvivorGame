using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EAugmentType
{
    MONSTERSPAWN = 1,
    PLAYERSTATUS = 2,
    MONSTERSTATUS = 3,
    OTHERAUGMENT = 4
}

public class AugmentManager : Singleton<AugmentManager>
{
    private Dictionary<int, AugmentData> augmentDataDict = new Dictionary<int, AugmentData>();
    private Dictionary<int, List<AugmentData>> augmentGroupDict = new Dictionary<int, List<AugmentData>>();
    
    private List<int> selectedAugmentUidList = new List<int>();
    private Dictionary<int, List<AugmentData>> curAugmentDict = new Dictionary<int, List<AugmentData>>();

    public Dictionary<int, Action> onRefreshAgumentActionDict = new Dictionary<int, Action>();


    public override bool Initialize()
    {
        InitAugmentDict();
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
            augmentData.augmentGroup = jsonData.BuildUpGruop;
            augmentData.augmentTier = jsonData.BuildUpGrade;
            augmentData.augmentName = jsonData.BuildUpName;
            augmentData.augmentImagePath = jsonData.BuildUpImage;
            augmentData.augmentInfo = jsonData.BuildUpContent;
            augmentData.firstAugmentType = (AugmentType)jsonData.BuildUpType;
            augmentData.firstAugmentValue = jsonData.BuildUpVariable;
            augmentData.secondAugmentType = (AugmentType)jsonData.BuildUpType2;
            augmentData.secondAugmentValue = jsonData.BuildUpVariavle2;

            augmentDataDict[augmentData.augmentUid] = augmentData;

            if (!augmentGroupDict.ContainsKey(augmentData.augmentGroup))
            {
                augmentGroupDict[augmentData.augmentGroup] = new List<AugmentData>();
            }

            augmentGroupDict[augmentData.augmentGroup].Add(augmentData);
        }
    }

    public void InitAugmentDict()
    {
        onRefreshAgumentActionDict.Add((int)EAugmentType.MONSTERSPAWN,() => { });
        onRefreshAgumentActionDict.Add((int)EAugmentType.PLAYERSTATUS, () => { });
        onRefreshAgumentActionDict.Add((int)EAugmentType.MONSTERSTATUS, () => { });
        onRefreshAgumentActionDict.Add((int)EAugmentType.OTHERAUGMENT, () => { });
    }

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

    public List<AugmentData> GetRandomAugment()
    {
        List<AugmentData> result = new List<AugmentData>();

        List<int> selectedGroupIds = new List<int>();
        List<int> selectedAugmentUids = new List<int>(selectedAugmentUidList);

        List<AugmentData> allAugments = augmentGroupDict.Values.SelectMany(group => group).ToList();
        allAugments = allAugments.OrderBy(a => Random.value).ToList();

        foreach (var augment in allAugments)
        {
            if (selectedAugmentUids.Contains(augment.augmentUid))
            {
                continue;
            }

            int groupCount = result.Count(a => a.augmentGroup == augment.augmentGroup);
            if (groupCount >= 1)
            {
                continue;
            }

            if (augment.isNotDuplicated && selectedGroupIds.Contains(augment.augmentGroup))
            {
                continue;
            }

            result.Add(augment);

            selectedGroupIds.Add(augment.augmentGroup);
            selectedAugmentUids.Add(augment.augmentUid);

            if (result.Count >= 3)
            {
                break;
            }
        }

        return result;
    }

    public void SelectAugment(int _augmentUid)
    {
        selectedAugmentUidList.Add(_augmentUid);

        AugmentData data = GetAugmentData(_augmentUid);

        int firstTypeNum = (int)data.firstAugmentType / 100;

        if (!curAugmentDict.ContainsKey(firstTypeNum))
        {
            curAugmentDict[firstTypeNum] = new List<AugmentData>();
        }

        curAugmentDict[firstTypeNum].Add(data);

        onRefreshAgumentActionDict[firstTypeNum]?.Invoke();

        if (data.secondAugmentType != 0)
        {
            int secondTypeNum = (int)data.secondAugmentType / 100;

            if (firstTypeNum != secondTypeNum)
            {
                onRefreshAgumentActionDict[secondTypeNum]?.Invoke();
            }
        }
    }

    public List<AugmentData> GetCurAugmentList(int _augmentType)
    {
        curAugmentDict.TryGetValue(_augmentType, out List<AugmentData> augmentList);

        if(augmentList == null)
        {
#if UNITY_EDITOR
            Debug.Log("Wrong Type Number");
#endif
            return null;
        }

        return augmentList;
    }
}
