using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildupData
{
    
}

public enum BuildUpType
{
    MONSTERSPAWN = 101,

    CHARACTERHP = 201,
    CHARACTERREGEN = 202,
    CHARACTERLIFESTEAL = 203,
    CHARACTERDAMAGE = 204,
    CHARACTERMELEEDAMAGE = 205,
    CHARACTERRANGEDAMAGE = 206,
    CHARACTERATTACKSPEED = 207,
    CHARACTERCRITICALCHANCE = 208,
    CHARACTERCRITICALDAMAGE = 209,
    CHARACTERMOVESPEED = 210,
    CHARACTERENGINEERING = 211,
    CHARACTERATTACKRANGE = 212,
    CHARACTERARMOUR = 213,
    CHARACTEREVASION = 214,
    CHARACTERLUCK = 215,
    CHARACTERPICKUPRANGE = 216,

    MONSTERHP = 301,
    MONSTERMOVESPEED = 302
}

public class JsonBuildupData
{
    public int BuildUpID;
    public int BuildUpGrade;
    public string BuildUpName;
    public string BuildUpImage;
    public string BuildUpContent;
    public int BuildUpType;
    public int BuildUpVariable;
    public int BuildUpType2;
    public int BuildUpVariavle2;
}
