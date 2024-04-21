using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    private CharacterModel characterModel;
    private BaseCharacterStatus[] characterStatus = new BaseCharacterStatus[(int)ECharacterStatus.END];

    public CharacterModel GetCharacterModel => characterModel;
    public BaseCharacterStatus GetPlayerStatus(ECharacterStatus _status) => characterStatus[(int)_status];

    public Character()
    {
        int count = (int)ECharacterStatus.END;

        for(int i = 0; i < count; i++)
        {
            characterStatus[i] = new BaseCharacterStatus();
        }

        characterStatus[(int)ECharacterStatus.MAXHP].baseStatus = 10;
        characterStatus[(int)ECharacterStatus.MOVE_SPEED].baseStatus = 5;
    }

    public void SetModel(CharacterModel _model)
    {
        characterModel = _model;
    }

    //List<스텟증감형장비아이템ID>
}

public enum ECharacterStatus
{
    MAXHP = 0,
    HP_REGEN,
    LIFE_STEAL,
    DAMAGE_MULITPLIER,
    MELEE_FLAT_DAMAGE,
    RANGE_FLAT_DAMAGE,
    ATTACK_SPEED,
    CRITICAL_CHANCE,
    ATTACK_RANGE,
    ARMOUR,
    EVASION,
    MOVE_SPEED,
    LUCK,
    HARVEST,
    END
}

public class CharacterModel
{
    public int characterUid;
    public string character_Name;
    public int unlockID;
    public int[] unique_Ability_IDArr;
    public string characterThumbnail;

    public List<Status_Variance> variances = new List<Status_Variance>();

}

public class Status_Variance
{
    public ECharacterStatus characterStatus;
    public float variance;
}

public class BaseCharacterStatus
{
    public float baseStatus;
    public float status_Multiplier;
    public float multiplier_Apply_status;
}