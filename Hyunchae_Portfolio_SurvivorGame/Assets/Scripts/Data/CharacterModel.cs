using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    private CharacterModel characterModel;
    private BaseCharacterStatus[] characterStatus = new BaseCharacterStatus[(int)ECharacterStatus.END];

    public Character()
    {
        int count = (int)ECharacterStatus.END;

        for(int i = 0; i < count; i++)
        {
            characterStatus[i] = new BaseCharacterStatus();
        }

        characterStatus[(int)ECharacterStatus.MAXHP].baseStatus = 10;
    }

    //무기아이템[]
    //List<장비아이템ID>
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
    public string character_thumbnail;

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

    //public float max_HP;
    //public float hp_Regen;
    //public float life_Steal;
    //public float damage_Multiplier;
    //public float melee_Flat_Damage;
    //public float range_Flat_Damage;
    //public float attack_Speed;
    //public float critical_Chance;
    //public float range;
    //public float armour;
    //public float evasion;
    //public float move_Speed;
    //public float luck;
    //public float harvest;

}