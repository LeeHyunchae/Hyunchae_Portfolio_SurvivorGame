using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    private const int BASEHP = 20;
    private const int BASEMOVESPEED = 5;
    private CharacterModel characterModel;
    private BaseCharacterStatus[] characterStatus = new BaseCharacterStatus[(int)ECharacterStatus.END];

    public CharacterModel GetCharacterModel => characterModel;
    public BaseCharacterStatus GetPlayerStatus(ECharacterStatus _status) => characterStatus[(int)_status];

    public Action<int> onRefreshStatusAction;

    public Character()
    {
        int count = (int)ECharacterStatus.END;

        for(int i = 0; i < count; i++)
        {
            characterStatus[i] = new BaseCharacterStatus();
        }
    }

    public void SetModel(CharacterModel _model)
    {
        characterModel = _model;

        ResetStatus();

        int count = characterModel.variances.Count;

        List<StatusVariance> variances = characterModel.variances;

        for (int i = 0; i < count; i++)
        {
            int statusNum = (int)variances[i].characterStatus;

            characterStatus[statusNum].baseStatus = variances[i].variance;
            characterStatus[statusNum].multiplierApplyStatus = variances[i].variance;
            characterStatus[statusNum].statusRatioMultiplier = 1;

        }

        characterStatus[(int)ECharacterStatus.PLAYER_MAXHP].baseStatus += BASEHP;
        characterStatus[(int)ECharacterStatus.PLAYER_MAXHP].multiplierApplyStatus = characterStatus[(int)ECharacterStatus.PLAYER_MAXHP].baseStatus;
        characterStatus[(int)ECharacterStatus.PLAYER_MOVE_SPEED].baseStatus += BASEMOVESPEED;
        characterStatus[(int)ECharacterStatus.PLAYER_MOVE_SPEED].multiplierApplyStatus = characterStatus[(int)ECharacterStatus.PLAYER_MOVE_SPEED].baseStatus;
    }

    public void ResetStatus()
    {
        int count = (int)ECharacterStatus.END;

        for (int i = 0; i < count; i++)
        {
            characterStatus[i] = new BaseCharacterStatus();
        }
    }

    public void UpdateStatusAmount(StatusVariance _variance)
    {
        BaseCharacterStatus status = characterStatus[(int)_variance.characterStatus];

        status.statusValueMultiplier += _variance.variance;

        status.multiplierApplyStatus =
            (status.baseStatus + status.statusValueMultiplier) * status.statusRatioMultiplier;

        onRefreshStatusAction?.Invoke((int)_variance.characterStatus);
    }

    public void UpdateStatusRatio(StatusVariance _variance)
    {
        BaseCharacterStatus status = characterStatus[(int)_variance.characterStatus];

        status.statusRatioMultiplier += _variance.variance;

        status.multiplierApplyStatus =
            (status.baseStatus + status.statusValueMultiplier) * status.statusRatioMultiplier;

        onRefreshStatusAction?.Invoke((int)_variance.characterStatus);
    }

    //List<스텟증감형장비아이템ID>
}

public enum ECharacterStatus
{
    PLAYER_MAXHP = 0,
    PLAYER_HP_REGEN,
    PLAYER_LIFE_STEAL,
    PLAYER_DAMAGE,
    PLAYER_MELEEDAMAGE,
    PLAYER_RANGEDAMAGE,
    PLAYER_ATTACKSPEED,
    PLAYER_CRITICALCHANCE,
    PLAYER_CRITICALLDAMAGE,
    PLAYER_MOVE_SPEED,
    PLAYER_ENGINEERING,
    PLAYER_ATTACK_RANGE,
    PLAYER_ARMOUR,
    PLAYER_EVASION,
    PLAYER_LUCK,
    PLAYER_PICKUPRANGE,
    PLAYER_HARVEST,
    END
}

public class CharacterModel
{
    public int characterUid;
    public string characterName;
    public int unlockID;
    public int[] uniqueAbilityIDArr;
    public string characterThumbnail;

    public List<StatusVariance> variances = new List<StatusVariance>();

}

public class StatusVariance
{
    public ECharacterStatus characterStatus;
    public bool isRatio;
    public float variance;
}

public class BaseCharacterStatus
{
    public float baseStatus;
    public float statusValueMultiplier;
    public float statusRatioMultiplier = 1;
    public float multiplierApplyStatus;
}