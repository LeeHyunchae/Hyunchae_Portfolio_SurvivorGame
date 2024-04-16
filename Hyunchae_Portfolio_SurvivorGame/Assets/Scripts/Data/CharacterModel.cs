using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECharacterStatus
{
    MAXHP = 0,
    HP_REGEN = 1,
    LIFE_STEAL = 2,
    DAMAGE_MAGNIFICATION = 3,
    MELEE_FLAT_DAMAGE = 4,
    RANGE_FLAT_DAMAGE = 5,
    ATTACK_SPEED = 6,
    CRITICAL_CHANCE = 7,
    RANGE = 8,
    ARMOUR = 9,
    EVASION = 10,
    MOVE_SPEED = 11,
    LUCK = 12,
    HARVEST = 13,
    END = 14
}

public class CharacterModel
{
    public string character_Name;
    public float[] character_StatusArr = new float[(int)ECharacterStatus.END];
    public string ability_Info;
    public string thumbnail_image;

    public List<CharacterStatus_Variance> variances = new List<CharacterStatus_Variance>();

    public CharacterModel()
    {
        character_StatusArr[(int)ECharacterStatus.MAXHP] = 10;
        character_StatusArr[(int)ECharacterStatus.HP_REGEN] = 0;
        character_StatusArr[(int)ECharacterStatus.LIFE_STEAL] = 0;
        character_StatusArr[(int)ECharacterStatus.DAMAGE_MAGNIFICATION] = 0;
        character_StatusArr[(int)ECharacterStatus.MELEE_FLAT_DAMAGE] = 0;
        character_StatusArr[(int)ECharacterStatus.RANGE_FLAT_DAMAGE] = 0;
        character_StatusArr[(int)ECharacterStatus.ATTACK_SPEED] = 0;
        character_StatusArr[(int)ECharacterStatus.CRITICAL_CHANCE] = 0;
        character_StatusArr[(int)ECharacterStatus.RANGE] = 0;
        character_StatusArr[(int)ECharacterStatus.ARMOUR] = 0;
        character_StatusArr[(int)ECharacterStatus.EVASION] = 0;
        character_StatusArr[(int)ECharacterStatus.MOVE_SPEED] = 0;
        character_StatusArr[(int)ECharacterStatus.LUCK] = 0;
        character_StatusArr[(int)ECharacterStatus.HARVEST] = 0;
    }
}

public class CharacterStatus_Variance
{
    public ECharacterStatus characterStatus;
    public float variance;
}