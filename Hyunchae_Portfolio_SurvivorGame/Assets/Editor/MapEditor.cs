using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(MapSetting))]
public class MapEditor : Editor
{
    private MapSetting mapSetting;
    private MapCreator mapCreator = new MapCreator();

    private Tilemap tilemap;

    public void OnEnable()
    {
        tilemap = FindObjectOfType<Tilemap>();
        mapSetting = target as MapSetting;
        mapCreator.Init(tilemap);
    }


    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- 맵 정보 입력 -");
        EditorGUILayout.Space();

        mapSetting.mapDatas.mapWidth = EditorGUILayout.IntField("맵 가로길이",mapSetting.mapDatas.mapWidth);
        mapSetting.mapDatas.mapHeight = EditorGUILayout.IntField("맵 세로길이", mapSetting.mapDatas.mapHeight);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("임시 맵 생성", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            mapCreator.GenerateMap(mapSetting.mapDatas.mapWidth,mapSetting.mapDatas.mapHeight);
        }

        if (GUILayout.Button("임시 맵 제거", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            tilemap.ClearAllTiles();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("맵 데이터 저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            SetTileData();

            TableLoader.SaveToJson("Map", mapSetting.mapDatas, "MapData");
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("저장된 맵 데이터 읽기", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            MapData mapData = TableLoader.LoadFromFile<MapData>("Map/MapData");

            mapSetting.mapDatas = mapData;

            mapCreator.GenerateMap(mapSetting.mapDatas.mapWidth, mapSetting.mapDatas.mapHeight);
        }

        if (GUILayout.Button("임시 캐릭터 데이터 저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            CharacterTest();
        }

        if (GUILayout.Button("임시 무기 데이터 저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            WeaponTest();
        }

        if (GUILayout.Button("임시 몬스터 데이터 저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            MonsterTest();
        }

        if (GUILayout.Button("임시 증강체 데이터 저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            AugmentTest();
        }

        if (GUILayout.Button("임시 스테이지 데이터 저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            StageTest();
        }

        //if (GUILayout.Button("맵 읽읽읽기", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        //{
        //    TableLoader tl = new TableLoader();

        //    TileBase[] allTile = tl.LoadFromFile<TileBase[]>("Map/AllTileMap");

        //}
    }

    private void SaveMapData()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        //TableLoader tl = new TableLoader();
        //tl.SaveToJson("Map", tilemap, "AllTileMap");

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    Debug.Log("Tile at position (" + (x + bounds.xMin) + ", " + (y + bounds.yMin) + "): " + tile.name);
                }
            }
        }
    }

    private void SetTileData()
    {
        int halfWidth = (int)(mapSetting.mapDatas.mapWidth * 0.5f);
        int halfHeight = (int)(mapSetting.mapDatas.mapHeight * 0.5f);

        TileData[] tileDatas = new TileData[mapSetting.mapDatas.mapWidth * mapSetting.mapDatas.mapHeight];

        int tileCount = 0;

        for (int i = -halfWidth; i < halfWidth; i++)
        {
            for (int j = -halfHeight; j < halfHeight; j++)
            {
                TileData tileData = new TileData();

                if(i == -halfWidth || i == halfWidth -1 || j == -halfHeight || j == halfHeight-1)
                {
                    tileData.isMove = false;
                }
                else
                {
                    tileData.isMove = true;
                }

                tileData.tilePos = new Vector3Int(i, j, 0);

                tileDatas[tileCount] = tileData;
                Debug.Log(tileCount);
                tileCount++;
            }
        }

        mapSetting.mapDatas.tileDatas = tileDatas;
    }

    private void CharacterTest()
    {
        List<CharacterModel> characters = new List<CharacterModel>();
        Status_Variance variance = new Status_Variance();

        CharacterModel characterModel = new CharacterModel
        {
            characterUid = 0,
            characterName = "테스트캐릭터",
            unlockID = 1,
            uniqueAbilityIDArr = new int[]{ 1, 2 },
            characterThumbnail = "Sprites/Enemy 0"
        };

        variance.characterStatus = ECharacterStatus.PLAYER_MAXHP;
        variance.variance = 5;

        characterModel.variances.Add(variance);
        variance = new Status_Variance();

        variance.characterStatus = ECharacterStatus.PLAYER_CRITICALCHANCE;
        variance.variance = 3;

        characterModel.variances.Add(variance);
        variance = new Status_Variance();

        variance.characterStatus = ECharacterStatus.PLAYER_ATTACKSPEED;
        variance.variance = -5;

        characterModel.variances.Add(variance);
        variance = new Status_Variance();

        variance.characterStatus = ECharacterStatus.PLAYER_MOVE_SPEED;
        variance.variance = 50;

        characterModel.variances.Add(variance);
        variance = new Status_Variance();

        characters.Add(characterModel);

        CharacterModel characterModel2 = new CharacterModel
        {
            characterUid = 1,
            characterName = "테스트캐릭터2",
            unlockID = 2,
            uniqueAbilityIDArr = new int[] { 3, 4 },
            characterThumbnail = "Sprites/Enemy 1"
        };

        variance.characterStatus = ECharacterStatus.PLAYER_MAXHP;
        variance.variance = -5;

        characterModel2.variances.Add(variance);
        variance = new Status_Variance();

        variance.characterStatus = ECharacterStatus.PLAYER_DAMAGE;
        variance.variance = 25;

        characterModel2.variances.Add(variance);
        variance = new Status_Variance();

        variance.characterStatus = ECharacterStatus.PLAYER_MOVE_SPEED;
        variance.variance = 30;

        characterModel2.variances.Add(variance);

        characters.Add(characterModel2);

        TableLoader.SaveToJson("Character", characters, "TestCharacter");
    }

    private void WeaponTest()
    {
        List<JsonWeaponData> weaponDatas = new List<JsonWeaponData>();

        JsonWeaponData jsonWeaponData1 = new JsonWeaponData
        {
            WeaponID = 0,
            WeaponGroup = 0,
            WeaponTier = 0,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.SHOOT,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 1,
            WeaponCritical = 1,
            WeaponTypeDamage = 1,
            WeaponRange = 10,
            WeaponSpeed = 5,
            WeaponCoolDown = 3,
            WeaponKnockback = 0,
            WeaponStatusEffect = 0,
            ItemImage = "Tier1_Props_3",
            BulletName = "Tier1_Props_9",
            ItemName = "Rifle"
        };

        weaponDatas.Add(jsonWeaponData1);

        JsonWeaponData jsonWeaponData2 = new JsonWeaponData
        {
            WeaponID = 1,
            WeaponGroup = 0,
            WeaponTier = 1,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.SHOOT,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 2,
            WeaponCritical = 2,
            WeaponTypeDamage = 2,
            WeaponRange = 10,
            WeaponSpeed = 5,
            WeaponCoolDown = 3,
            WeaponKnockback = 0,
            WeaponStatusEffect = 0,
            ItemImage = "Tier2_Props_3",
            BulletName = "Tier1_Props_9",
            ItemName = "Rifle2"
        };

        weaponDatas.Add(jsonWeaponData2);

        JsonWeaponData jsonWeaponData3 = new JsonWeaponData
        {
            WeaponID = 2,
            WeaponGroup = 0,
            WeaponTier = 2,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.SHOOT,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 3,
            WeaponCritical = 3,
            WeaponTypeDamage = 3,
            WeaponRange = 10,
            WeaponSpeed = 5,
            WeaponCoolDown = 3,
            WeaponKnockback = 0,
            WeaponStatusEffect = 0,
            ItemImage = "Tier3_Props_3",
            BulletName = "Tier1_Props_9",
            ItemName = "Rifle3"
        };

        weaponDatas.Add(jsonWeaponData3);

        JsonWeaponData jsonWeaponData4 = new JsonWeaponData
        {
            WeaponID = 3,
            WeaponGroup = 0,
            WeaponTier = 3,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.SHOOT,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 4,
            WeaponCritical = 4,
            WeaponTypeDamage = 4,
            WeaponRange = 10,
            WeaponSpeed = 5,
            WeaponCoolDown = 3,
            WeaponKnockback = 0,
            WeaponStatusEffect = 0,
            ItemImage = "Tier4_Props_3",
            BulletName = "Tier1_Props_9",
            ItemName = "Rifle4"
        };

        weaponDatas.Add(jsonWeaponData4);

        jsonWeaponData1 = new JsonWeaponData
        {
            WeaponID = 4,
            WeaponGroup = 1,
            WeaponTier = 0,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.STING,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 1,
            WeaponCritical = 1,
            WeaponTypeDamage = 1,
            WeaponRange = 5,
            WeaponSpeed = 5,
            WeaponCoolDown = 2,
            WeaponKnockback = 0,
            WeaponStatusEffect = 0,
            ItemImage = "Tier1_Props_1",
            BulletName = "",
            ItemName = "Spear"
        };

        weaponDatas.Add(jsonWeaponData1);

        jsonWeaponData2 = new JsonWeaponData
        {
            WeaponID = 5,
            WeaponGroup = 1,
            WeaponTier = 1,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.STING,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 2,
            WeaponCritical = 2,
            WeaponTypeDamage = 2,
            WeaponRange = 5,
            WeaponSpeed = 5,
            WeaponCoolDown = 2,
            WeaponKnockback = 0,
            WeaponStatusEffect = 0,
            ItemImage = "Tier2_Props_1",
            BulletName = "",
            ItemName = "Spear2"
        };

        weaponDatas.Add(jsonWeaponData2);

        jsonWeaponData3 = new JsonWeaponData
        {
            WeaponID = 6,
            WeaponGroup = 1,
            WeaponTier = 2,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.STING,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 3,
            WeaponCritical = 3,
            WeaponTypeDamage = 3,
            WeaponRange = 5,
            WeaponSpeed = 5,
            WeaponCoolDown = 2,
            WeaponKnockback = 0,
            WeaponStatusEffect = 0,
            ItemImage = "Tier3_Props_1",
            BulletName = "",
            ItemName = "Spear3"
        };

        weaponDatas.Add(jsonWeaponData3);

        jsonWeaponData4 = new JsonWeaponData
        {
            WeaponID = 7,
            WeaponGroup = 1,
            WeaponTier = 3,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.STING,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 4,
            WeaponCritical = 4,
            WeaponTypeDamage = 4,
            WeaponRange = 5,
            WeaponSpeed = 5,
            WeaponCoolDown = 2,
            WeaponKnockback = 0,
            WeaponStatusEffect = 0,
            ItemImage = "Tier4_Props_1",
            BulletName = "",
            ItemName = "Spear4"
        };

        weaponDatas.Add(jsonWeaponData4);

        jsonWeaponData1 = new JsonWeaponData
        {
            WeaponID = 8,
            WeaponGroup = 2,
            WeaponTier = 0,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.SWING,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 1,
            WeaponCritical = 1,
            WeaponTypeDamage = 1,
            WeaponRange = 5,
            WeaponSpeed = 5,
            WeaponCoolDown = 2,
            WeaponKnockback = 0,
            WeaponStatusEffect = 0,
            ItemImage = "Tier1_Props_2",
            BulletName = "",
            ItemName = "Scythe"
        };

        weaponDatas.Add(jsonWeaponData1);

        jsonWeaponData2 = new JsonWeaponData
        {
            WeaponID = 9,
            WeaponGroup = 2,
            WeaponTier = 1,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.SWING,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 2,
            WeaponCritical = 2,
            WeaponTypeDamage = 2,
            WeaponRange = 5,
            WeaponSpeed = 5,
            WeaponCoolDown = 2,
            WeaponKnockback = 0,
            WeaponStatusEffect = 0,
            ItemImage = "Tier2_Props_2",
            BulletName = "",
            ItemName = "Scythe2"
        };

        weaponDatas.Add(jsonWeaponData2);

        jsonWeaponData3 = new JsonWeaponData
        {
            WeaponID = 10,
            WeaponGroup = 2,
            WeaponTier = 2,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.SWING,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 3,
            WeaponCritical = 3,
            WeaponTypeDamage = 3,
            WeaponRange = 5,
            WeaponSpeed = 5,
            WeaponCoolDown = 2,
            WeaponKnockback = 0,
            WeaponStatusEffect = 0,
            ItemImage = "Tier3_Props_2",
            BulletName = "",
            ItemName = "Scythe3"
        };

        weaponDatas.Add(jsonWeaponData3);

        jsonWeaponData4 = new JsonWeaponData
        {
            WeaponID = 11,
            WeaponGroup = 2,
            WeaponTier = 3,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.SWING,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 4,
            WeaponCritical = 4,
            WeaponTypeDamage = 4,
            WeaponRange = 5,
            WeaponSpeed = 5,
            WeaponCoolDown = 2,
            WeaponKnockback = 0,
            WeaponStatusEffect = 0,
            ItemImage = "Tier4_Props_2",
            BulletName = "",
            ItemName = "Scythe4"
        };

        weaponDatas.Add(jsonWeaponData4);

        TableLoader.SaveToJson("Weapon", weaponDatas, "TestWeapon");


        //List<WeaponItemModel> itemModels = new List<WeaponItemModel>();
        //WeaponItemModel itemModel = new WeaponItemModel
        //{
        //    itemUid = 0,
        //    itemType = EItemType.ATTACKABLE,
        //    itemPrice = 3,
        //    itemThumbnail = "Weapon 3",
        //    bulletImage = "Bullet 3",
        //    itemName = "Gun",
        //    WeaponType = EWeaponType.SHOOT,
        //    uniqueAbilityIDArr = new int[] { 1, 2, 3 }

        //};

        //WeaponStatus weaponStatus = new WeaponStatus();
        //weaponStatus.damage = 3;
        //weaponStatus.cooldown = 3;

        //itemModel.status = weaponStatus;

        //itemModels.Add(itemModel);

        //WeaponItemModel itemModel2 = new WeaponItemModel
        //{
        //    itemUid = 1,
        //    itemType = EItemType.ATTACKABLE,
        //    itemPrice = 8,
        //    itemThumbnail = "Weapon 1",
        //    bulletImage = string.Empty,
        //    itemName = "Spear",
        //    WeaponType = EWeaponType.STING,
        //    uniqueAbilityIDArr = new int[] { 3, 4, 5 }

        //};

        //WeaponStatus weaponStatus2 = new WeaponStatus();
        //weaponStatus.damage = 5;
        //weaponStatus.cooldown = 5;

        //itemModel2.status = weaponStatus2;

        //itemModels.Add(itemModel2);

        //WeaponItemModel itemModel3 = new WeaponItemModel
        //{
        //    itemUid = 2,
        //    itemType = EItemType.ATTACKABLE,
        //    itemPrice = 8,
        //    itemThumbnail = "Weapon 2",
        //    bulletImage = string.Empty,
        //    itemName = "Scythe",
        //    WeaponType = EWeaponType.SWING,
        //    uniqueAbilityIDArr = new int[] { 3, 4, 5 }

        //};

        //WeaponStatus weaponStatus3 = new WeaponStatus();
        //weaponStatus.damage = 5;
        //weaponStatus.cooldown = 5;

        //itemModel2.status = weaponStatus3;

        //itemModels.Add(itemModel3);

        //TableLoader.SaveToJson("Weapon", itemModels, "TestWeapon");
    }

    private void MonsterTest()
    {
        List<MonsterModel> monsterModels = new List<MonsterModel>();

        MonsterModel monsterModel1 = new MonsterModel()
        {
            monsterUid = 0,
            monsterName = "FollowMon",
            monsterThumbnail = "Enemy 0",
            logicType = EMonsterLogicType.LOOP,
            skillType = EMonsterSkillType.NONE,
            moveType = EMonsterMoveType.FOLLOW,
            monsterStatus = new float[] { 10, 1, 5, 2, 5 }
        };

        monsterModels.Add(monsterModel1);

        MonsterModel monsterModel2 = new MonsterModel()
        {
            monsterUid = 1,
            monsterName = "DashSeqMon",
            monsterThumbnail = "Enemy 1",
            logicType = EMonsterLogicType.SEQUENCE,
            skillType = EMonsterSkillType.DASH,
            moveType = EMonsterMoveType.FOLLOW,
            monsterStatus = new float[] { 10,1,5,2,5}
        };

        monsterModels.Add(monsterModel2);

        MonsterModel monsterModel3 = new MonsterModel()
        {
            monsterUid = 2,
            monsterName = "ShootingLoopMon",
            monsterThumbnail = "Enemy 2",
            logicType = EMonsterLogicType.LOOP,
            skillType = EMonsterSkillType.SHOOTING,
            moveType = EMonsterMoveType.AWAY,
            monsterStatus = new float[] { 10, 1, 5, 2, 5 }
        };

        monsterModels.Add(monsterModel3);

        TableLoader.SaveToJson("Monster", monsterModels, "TestMonster");

    }

    private void StageTest()
    {
        StageData[] stageDatas = new StageData[6];

        for(int i = 0; i< 6;i++)
        {
            StageData stage = new StageData()
            {
                StageID = i,
                StageInformation = i + " 스테이지",
                WaveMonsterGroupID = new int[] { 0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, 0 },
                BossMonsterID = 0
            };
            stageDatas[i] = stage;
        }

        TableLoader.SaveToJson("Stage", stageDatas, "TestStage");

        JsonMonsterGroupData[] jsonMonsterGroupDatas = new JsonMonsterGroupData[6];

        jsonMonsterGroupDatas[0] = new JsonMonsterGroupData()
        {
            MonsterGroupID = 0,
            MonsterID = 0,
            MonsterNumber = 1,
            FirstSpawnTime = 3,
            EndSpawnTime = 90,
            RespawnCycle = 5
        };

        jsonMonsterGroupDatas[1] = new JsonMonsterGroupData()
        {
            MonsterGroupID = 0,
            MonsterID = 0,
            MonsterNumber = 4,
            FirstSpawnTime = 5,
            EndSpawnTime = 90,
            RespawnCycle = 4
        };

        jsonMonsterGroupDatas[2] = new JsonMonsterGroupData()
        {
            MonsterGroupID = 1,
            MonsterID = 0,
            MonsterNumber = 4,
            FirstSpawnTime = 3,
            EndSpawnTime = 90,
            RespawnCycle = 5
        };

        jsonMonsterGroupDatas[3] = new JsonMonsterGroupData()
        {
            MonsterGroupID = 1,
            MonsterID = 2,
            MonsterNumber = 1,
            FirstSpawnTime = 5,
            EndSpawnTime = 90,
            RespawnCycle = 3
        };

        jsonMonsterGroupDatas[4] = new JsonMonsterGroupData()
        {
            MonsterGroupID = 2,
            MonsterID = 0,
            MonsterNumber = 1,
            FirstSpawnTime = 3,
            EndSpawnTime = 90,
            RespawnCycle = 5
        };

        jsonMonsterGroupDatas[5] = new JsonMonsterGroupData()
        {
            MonsterGroupID = 2,
            MonsterID = 2,
            MonsterNumber = 4,
            FirstSpawnTime = 5,
            EndSpawnTime = 90,
            RespawnCycle = 2
        };

        TableLoader.SaveToJson("Stage", jsonMonsterGroupDatas, "TestMonsterGroup");

    }


    private void AugmentTest()
    {
        List<JsonAugmentData> augmentDatas = new List<JsonAugmentData>();

        JsonAugmentData augmentData = new JsonAugmentData
        {
            BuildUpID = 6000001,
            BuildUpGrade = 1,
            BuildUpName = "UI_Text_BuildUp_Name_6000001",
            BuildUpImage = "BuildUpImage_6000001.png",
            BuildUpContent = "Mosnter SpawnTime+",
            BuildUpType = 101,
            BuildUpVariable = 10,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 0
        };

        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000002,
            BuildUpGrade = 2,
            BuildUpName = "UI_Text_BuildUp_Name_6000002",
            BuildUpImage = "BuildUpImage_6000002.png",
            BuildUpContent = "Mosnter SpawnTime++",
            BuildUpType = 101,
            BuildUpVariable = 25,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 0
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000003,
            BuildUpGrade = 3,
            BuildUpName = "UI_Text_BuildUp_Name_6000003",
            BuildUpImage = "BuildUpImage_6000003.png",
            BuildUpContent = "Mosnter SpawnTime+++",
            BuildUpType = 101,
            BuildUpVariable = 50,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 0
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000004,
            BuildUpGrade = 1,
            BuildUpName = "UI_Text_BuildUp_Name_6000004",
            BuildUpImage = "BuildUpImage_6000004.png",
            BuildUpContent = "Mosnter SpawnTime-",
            BuildUpType = 101,
            BuildUpVariable = -10,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 1
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000005,
            BuildUpGrade = 2,
            BuildUpName = "UI_Text_BuildUp_Name_6000005",
            BuildUpImage = "BuildUpImage_6000005.png",
            BuildUpContent = "Mosnter SpawnTime--",
            BuildUpType = 101,
            BuildUpVariable = -25,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 1
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000006,
            BuildUpGrade = 3,
            BuildUpName = "UI_Text_BuildUp_Name_6000006",
            BuildUpImage = "BuildUpImage_6000006.png",
            BuildUpContent = "Mosnter SpawnTime---",
            BuildUpType = 101,
            BuildUpVariable = -50,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 1
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000007,
            BuildUpGrade = 1,
            BuildUpName = "UI_Text_BuildUp_Name_6000007",
            BuildUpImage = "BuildUpImage_6000007.png",
            BuildUpContent = "Mosnter MoveSpeed-",
            BuildUpType = 302,
            BuildUpVariable = 10,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 2
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000008,
            BuildUpGrade = 2,
            BuildUpName = "UI_Text_BuildUp_Name_6000008",
            BuildUpImage = "BuildUpImage_6000008.png",
            BuildUpContent = "Mosnter MoveSpeed--, Monster HP+",
            BuildUpType = 302,
            BuildUpVariable = 25,
            BuildUpType2 = 301,
            BuildUpVariavle2 = 10,
            BuildUpGruop = 2
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000009,
            BuildUpGrade = 3,
            BuildUpName = "UI_Text_BuildUp_Name_6000009",
            BuildUpImage = "BuildUpImage_6000009.png",
            BuildUpContent = "Mosnter MoveSpeed---, Monster HP++",
            BuildUpType = 302,
            BuildUpVariable = 50,
            BuildUpType2 = 301,
            BuildUpVariavle2 = 25,
            BuildUpGruop = 2
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000010,
            BuildUpGrade = 1,
            BuildUpName = "UI_Text_BuildUp_Name_6000010",
            BuildUpImage = "BuildUpImage_6000010.png",
            BuildUpContent = "Player Max HP +",
            BuildUpType = 201,
            BuildUpVariable = 10,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 3
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000011,
            BuildUpGrade = 2,
            BuildUpName = "UI_Text_BuildUp_Name_6000011",
            BuildUpImage = "BuildUpImage_6000011.png",
            BuildUpContent = "Player Max HP ++",
            BuildUpType = 201,
            BuildUpVariable = 25,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 3
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000012,
            BuildUpGrade = 3,
            BuildUpName = "UI_Text_BuildUp_Name_6000012",
            BuildUpImage = "BuildUpImage_6000012.png",
            BuildUpContent = "Player Max HP +++",
            BuildUpType = 201,
            BuildUpVariable = 50,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 3
        };
        augmentDatas.Add(augmentData);

        TableLoader.SaveToJson("Augment", augmentDatas, "TestAugment");

    }
}
