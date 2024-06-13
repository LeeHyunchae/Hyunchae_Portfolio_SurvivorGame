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

        characterStatus[(int)ECharacterStatus.PLAYER_MAXHP].baseStatus = 10;
        characterStatus[(int)ECharacterStatus.PLAYER_MOVE_SPEED].baseStatus = 5;
    }

    public void SetModel(CharacterModel _model)
    {
        characterModel = _model;
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
    public float statusMultiplier;
    public float multiplierApplyStatus;
}