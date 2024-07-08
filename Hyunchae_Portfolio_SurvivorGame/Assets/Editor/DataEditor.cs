using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(MapSetting))]
public class DataEditor : Editor
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

        if (GUILayout.Button("맵 미리보기 생성", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            mapCreator.GenerateMap(mapSetting.mapDatas.mapWidth,mapSetting.mapDatas.mapHeight);
        }

        if (GUILayout.Button("저장된 맵 데이터 제거", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
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

        if (GUILayout.Button("캐릭터 데이터 저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            CharacterDataEdit();
        }

        if (GUILayout.Button("무기 데이터 저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            WeaponDataEdit();
        }

        if (GUILayout.Button("몬스터 데이터 저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            MonsterDataEdit();
        }

        if (GUILayout.Button("증강체 데이터 저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            AugmentDataEdit();
        }

        if (GUILayout.Button("패시브 아이템 데이터 저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            PassiveItemDataEdit();
        }

        if (GUILayout.Button("보스 몬스터 데이터 저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            BossMonsterDataEdit();
        }

        if (GUILayout.Button("스테이지 데이터 저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            StageDataEdit();
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

    private void CharacterDataEdit()
    {
        List<CharacterModel> characters = new List<CharacterModel>();
        StatusVariance variance = new StatusVariance();

        CharacterModel characterModel = new CharacterModel
        {
            characterUid = 0,
            characterName = "테스트캐릭터",
            unlockID = 1,
            uniqueAbilityIDArr = new int[]{ 1, 2 },
            characterThumbnail = "Sprites/Character_1"
        };

        variance.characterStatus = ECharacterStatus.PLAYER_MAXHP;
        variance.variance = 25;
        variance.isRatio = false;

        characterModel.variances.Add(variance);
        variance = new StatusVariance();

        variance.characterStatus = ECharacterStatus.PLAYER_CRITICALCHANCE;
        variance.variance = 3;
        variance.isRatio = false;

        characterModel.variances.Add(variance);
        variance = new StatusVariance();

        variance.characterStatus = ECharacterStatus.PLAYER_ATTACKSPEED;
        variance.variance = -5;
        variance.isRatio = false;

        characterModel.variances.Add(variance);
        variance = new StatusVariance();

        variance.characterStatus = ECharacterStatus.PLAYER_MOVE_SPEED;
        variance.variance = 50;
        variance.isRatio = false;

        characterModel.variances.Add(variance);
        variance = new StatusVariance();

        characters.Add(characterModel);

        CharacterModel characterModel2 = new CharacterModel
        {
            characterUid = 1,
            characterName = "테스트캐릭터2",
            unlockID = 2,
            uniqueAbilityIDArr = new int[] { 3, 4 },
            characterThumbnail = "Sprites/Character_2"
        };

        variance.characterStatus = ECharacterStatus.PLAYER_MAXHP;
        variance.variance = 15;
        variance.isRatio = false;

        characterModel2.variances.Add(variance);
        variance = new StatusVariance();

        variance.characterStatus = ECharacterStatus.PLAYER_DAMAGE;
        variance.variance = 25;
        variance.isRatio = false;

        characterModel2.variances.Add(variance);
        variance = new StatusVariance();
        variance.isRatio = false;
        variance.characterStatus = ECharacterStatus.PLAYER_MOVE_SPEED;
        variance.variance = 30;

        characterModel2.variances.Add(variance);

        characters.Add(characterModel2);

        CharacterModel characterModel3 = new CharacterModel
        {
            characterUid = 2,
            characterName = "테스트캐릭터3",
            unlockID = 2,
            uniqueAbilityIDArr = new int[] { 3, 4 },
            characterThumbnail = "Sprites/Character_3"
        };

        variance.characterStatus = ECharacterStatus.PLAYER_MAXHP;
        variance.variance = 100;
        variance.isRatio = false;

        characterModel3.variances.Add(variance);
        variance = new StatusVariance();

        variance.characterStatus = ECharacterStatus.PLAYER_ATTACK_RANGE;
        variance.variance = 25;
        variance.isRatio = false;

        characterModel3.variances.Add(variance);
        variance = new StatusVariance();
        variance.isRatio = false;
        variance.characterStatus = ECharacterStatus.PLAYER_MOVE_SPEED;
        variance.variance = -5;

        characterModel3.variances.Add(variance);

        characters.Add(characterModel3);

        CharacterModel characterModel4 = new CharacterModel
        {
            characterUid = 3,
            characterName = "테스트캐릭터4",
            unlockID = 2,
            uniqueAbilityIDArr = new int[] { 3, 4 },
            characterThumbnail = "Sprites/Character_4"
        };

        variance.characterStatus = ECharacterStatus.PLAYER_MAXHP;
        variance.variance = 25;
        variance.isRatio = false;

        characterModel4.variances.Add(variance);
        variance = new StatusVariance();

        variance.characterStatus = ECharacterStatus.PLAYER_ATTACKSPEED;
        variance.variance = 25;
        variance.isRatio = false;

        characterModel4.variances.Add(variance);
        variance = new StatusVariance();
        variance.isRatio = false;
        variance.characterStatus = ECharacterStatus.PLAYER_MOVE_SPEED;
        variance.variance = 25;

        characterModel4.variances.Add(variance);

        characters.Add(characterModel4);

        TableLoader.SaveToJson("Character", characters, "TestCharacter");
    }

    private void WeaponDataEdit()
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
            WeaponDamage = 5,
            WeaponCritical = 1,
            WeaponTypeDamage = 1,
            WeaponRange = 10,
            WeaponSpeed = 5,
            WeaponCoolDown = 0.5f,
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier1_Props_3",
            BulletName = "Tier1_Props_9",
            ItemName = "Rifle",
            ItemPrice = 2
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
            WeaponDamage = 6,
            WeaponCritical = 2,
            WeaponTypeDamage = 2,
            WeaponRange = 10,
            WeaponSpeed = 5,
            WeaponCoolDown = 0.5f,
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier2_Props_3",
            BulletName = "Tier1_Props_9",
            ItemName = "Rifle2",
            ItemPrice = 4
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
            WeaponDamage = 7,
            WeaponCritical = 3,
            WeaponTypeDamage = 3,
            WeaponRange = 10,
            WeaponSpeed = 5,
            WeaponCoolDown = 0.5f,
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier3_Props_3",
            BulletName = "Tier1_Props_9",
            ItemName = "Rifle3",
            ItemPrice = 6
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
            WeaponDamage = 8,
            WeaponCritical = 4,
            WeaponTypeDamage = 4,
            WeaponRange = 10,
            WeaponSpeed = 5,
            WeaponCoolDown = 0.5f,
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier4_Props_3",
            BulletName = "Tier1_Props_9",
            ItemName = "Rifle4",
            ItemPrice = 8
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
            WeaponDamage = 5,
            WeaponCritical = 1,
            WeaponTypeDamage = 1,
            WeaponRange = 4,
            WeaponSpeed = 5,
            WeaponCoolDown = 1,
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier1_Props_1",
            BulletName = "",
            ItemName = "Spear",
            ItemPrice = 2
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
            WeaponDamage = 6,
            WeaponCritical = 2,
            WeaponTypeDamage = 2,
            WeaponRange = 4,
            WeaponSpeed = 5,
            WeaponCoolDown = 1,
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier2_Props_1",
            BulletName = "",
            ItemName = "Spear2",
            ItemPrice = 4
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
            WeaponDamage = 7,
            WeaponCritical = 3,
            WeaponTypeDamage = 3,
            WeaponRange = 4,
            WeaponSpeed = 5,
            WeaponCoolDown = 1,
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier3_Props_1",
            BulletName = "",
            ItemName = "Spear3",
            ItemPrice = 6
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
            WeaponDamage = 8,
            WeaponCritical = 4,
            WeaponTypeDamage = 4,
            WeaponRange = 4,
            WeaponSpeed = 5,
            WeaponCoolDown = 1,
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier4_Props_1",
            BulletName = "",
            ItemName = "Spear4",
            ItemPrice = 8
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
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier1_Props_2",
            BulletName = "",
            ItemName = "Scythe",
            ItemPrice = 2
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
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier2_Props_2",
            BulletName = "",
            ItemName = "Scythe2",
            ItemPrice = 4
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
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier3_Props_2",
            BulletName = "",
            ItemName = "Scythe3",
            ItemPrice = 6
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
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier4_Props_2",
            BulletName = "",
            ItemName = "Scythe4",
            ItemPrice = 8
        };

        weaponDatas.Add(jsonWeaponData4);

        jsonWeaponData1 = new JsonWeaponData
        {
            WeaponID = 12,
            WeaponGroup = 3,
            WeaponTier = 0,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.STING,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 5,
            WeaponCritical = 2,
            WeaponTypeDamage = 2,
            WeaponRange = 2,
            WeaponSpeed = 6,
            WeaponCoolDown = 1,
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier1_Props_0",
            BulletName = "",
            ItemName = "Shovel",
            ItemPrice = 2
        };

        weaponDatas.Add(jsonWeaponData1);

        jsonWeaponData2 = new JsonWeaponData
        {
            WeaponID = 13,
            WeaponGroup = 3,
            WeaponTier = 1,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.STING,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 6,
            WeaponCritical = 2,
            WeaponTypeDamage = 2,
            WeaponRange = 2,
            WeaponSpeed = 6,
            WeaponCoolDown = 1,
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier2_Props_0",
            BulletName = "",
            ItemName = "Shovel2",
            ItemPrice = 4
        };

        weaponDatas.Add(jsonWeaponData2);

        jsonWeaponData3 = new JsonWeaponData
        {
            WeaponID = 14,
            WeaponGroup = 3,
            WeaponTier = 2,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.STING,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 7,
            WeaponCritical = 2,
            WeaponTypeDamage = 2,
            WeaponRange = 2,
            WeaponSpeed = 6,
            WeaponCoolDown = 1,
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier3_Props_0",
            BulletName = "",
            ItemName = "Shovel3",
            ItemPrice = 6
        };

        weaponDatas.Add(jsonWeaponData3);

        jsonWeaponData4 = new JsonWeaponData
        {
            WeaponID = 15,
            WeaponGroup = 3,
            WeaponTier = 3,
            WeaponSynergy = 0,
            WeaponType = EWeaponType.STING,
            WeaponAttackType = EWeaponAttackType.NONE,
            WeaponDamage = 8,
            WeaponCritical = 2,
            WeaponTypeDamage = 2,
            WeaponRange = 2,
            WeaponSpeed = 6,
            WeaponCoolDown = 1,
            WeaponKnockback = 0.3f,
            WeaponStatusEffect = 0,
            ItemImage = "Tier4_Props_0",
            BulletName = "",
            ItemName = "Shovel4",
            ItemPrice = 8
        };

        weaponDatas.Add(jsonWeaponData4);

        TableLoader.SaveToJson("Weapon", weaponDatas, "TestWeapon");
    }

    private void MonsterDataEdit()
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
            monsterStatus = new float[] { 7, 1, 3, 2, 5 },
            dropPieceCount = 3
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
            monsterStatus = new float[] { 10,1,3,2,5},
            dropPieceCount = 3
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
            monsterStatus = new float[] { 7, 1, 3, 2, 5 },
            dropPieceCount = 5
        };

        monsterModels.Add(monsterModel3);

        TableLoader.SaveToJson("Monster", monsterModels, "TestMonster");

    }

    private void StageDataEdit()
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
            MonsterNumber = 2,
            FirstSpawnTime = 5,
            EndSpawnTime = 90,
            RespawnCycle = 4
        };

        jsonMonsterGroupDatas[2] = new JsonMonsterGroupData()
        {
            MonsterGroupID = 1,
            MonsterID = 0,
            MonsterNumber = 2,
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
            MonsterNumber = 2,
            FirstSpawnTime = 3,
            EndSpawnTime = 90,
            RespawnCycle = 5
        };

        jsonMonsterGroupDatas[5] = new JsonMonsterGroupData()
        {
            MonsterGroupID = 2,
            MonsterID = 2,
            MonsterNumber = 2,
            FirstSpawnTime = 5,
            EndSpawnTime = 90,
            RespawnCycle = 2
        };

        TableLoader.SaveToJson("Stage", jsonMonsterGroupDatas, "TestMonsterGroup");

    }


    private void AugmentDataEdit()
    {
        List<JsonAugmentData> augmentDatas = new List<JsonAugmentData>();

        JsonAugmentData augmentData = new JsonAugmentData
        {
            BuildUpID = 6000001,
            BuildUpGrade = 1,
            BuildUpName = "유토피아",
            BuildUpImage = "BuildUpImage_6000001.png",
            BuildUpContent = "Mosnter SpawnTime+",
            BuildUpType = 101,
            BuildUpVariable = 10,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 0,
            IsNotDuplicated = true
        };

        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000002,
            BuildUpGrade = 2,
            BuildUpName = "유토피아+",
            BuildUpImage = "BuildUpImage_6000002.png",
            BuildUpContent = "Mosnter SpawnTime++",
            BuildUpType = 101,
            BuildUpVariable = 25,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 0,
            IsNotDuplicated = true
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000003,
            BuildUpGrade = 3,
            BuildUpName = "유토피아++",
            BuildUpImage = "BuildUpImage_6000003.png",
            BuildUpContent = "Mosnter SpawnTime+++",
            BuildUpType = 101,
            BuildUpVariable = 50,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 0,
            IsNotDuplicated = true
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000004,
            BuildUpGrade = 1,
            BuildUpName = "디스토피아",
            BuildUpImage = "BuildUpImage_6000004.png",
            BuildUpContent = "Mosnter SpawnTime-",
            BuildUpType = 101,
            BuildUpVariable = -10,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 1,
            IsNotDuplicated = true
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000005,
            BuildUpGrade = 2,
            BuildUpName = "디스토피아+",
            BuildUpImage = "BuildUpImage_6000005.png",
            BuildUpContent = "Mosnter SpawnTime--",
            BuildUpType = 101,
            BuildUpVariable = -25,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 1,
            IsNotDuplicated = true
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000006,
            BuildUpGrade = 3,
            BuildUpName = "디스토피아++",
            BuildUpImage = "BuildUpImage_6000006.png",
            BuildUpContent = "Mosnter SpawnTime---",
            BuildUpType = 101,
            BuildUpVariable = -50,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 1,
            IsNotDuplicated = true
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000007,
            BuildUpGrade = 1,
            BuildUpName = "안전한 삶",
            BuildUpImage = "BuildUpImage_6000007.png",
            BuildUpContent = "Mosnter MoveSpeed-",
            BuildUpType = 302,
            BuildUpVariable = 10,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 2,
            IsNotDuplicated = false
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000008,
            BuildUpGrade = 2,
            BuildUpName = "안전한 삶+",
            BuildUpImage = "BuildUpImage_6000008.png",
            BuildUpContent = "Mosnter MoveSpeed--, Monster HP+",
            BuildUpType = 302,
            BuildUpVariable = 25,
            BuildUpType2 = 301,
            BuildUpVariavle2 = 10,
            BuildUpGruop = 2,
            IsNotDuplicated = false
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000009,
            BuildUpGrade = 3,
            BuildUpName = "안전한 삶++",
            BuildUpImage = "BuildUpImage_6000009.png",
            BuildUpContent = "Mosnter MoveSpeed---, Monster HP++",
            BuildUpType = 302,
            BuildUpVariable = 50,
            BuildUpType2 = 301,
            BuildUpVariavle2 = 25,
            BuildUpGruop = 2,
            IsNotDuplicated = false
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000010,
            BuildUpGrade = 1,
            BuildUpName = "튼튼한 몸",
            BuildUpImage = "BuildUpImage_6000010.png",
            BuildUpContent = "Player Max HP +",
            BuildUpType = 201,
            BuildUpVariable = 10,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 3,
            IsNotDuplicated = false
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000011,
            BuildUpGrade = 2,
            BuildUpName = "튼튼한 몸+",
            BuildUpImage = "BuildUpImage_6000011.png",
            BuildUpContent = "Player Max HP ++",
            BuildUpType = 201,
            BuildUpVariable = 25,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 3,
            IsNotDuplicated = false
        };
        augmentDatas.Add(augmentData);

        augmentData = new JsonAugmentData
        {
            BuildUpID = 6000012,
            BuildUpGrade = 3,
            BuildUpName = "튼튼한 몸++",
            BuildUpImage = "BuildUpImage_6000012.png",
            BuildUpContent = "Player Max HP +++",
            BuildUpType = 201,
            BuildUpVariable = 50,
            BuildUpType2 = 0,
            BuildUpVariavle2 = 0,
            BuildUpGruop = 3,
            IsNotDuplicated = false
        };
        augmentDatas.Add(augmentData);

        TableLoader.SaveToJson("Augment", augmentDatas, "TestAugment");

    }

    public void PassiveItemDataEdit()
    {
        List<JsonPassiveItemModel> itemDatas = new List<JsonPassiveItemModel>();

        List<ItemStatusVariance> status_Variances = new List<ItemStatusVariance>();
        ItemStatusVariance status_Variance;
        JsonPassiveItemModel model = new JsonPassiveItemModel()
        {
            ItemID = 1000,
            ItemTier = 0,
            ItemPassiveType = EPassiveItemType.SUPPORTABLE,
            ItemImage = "Select 6",
            BulletName = "",
            ItemName = "아이템 1",
            ItemContent = "방어력 상승 이동속도 저하 아이템",
            ItemPrice = 3
        };

        status_Variance = new ItemStatusVariance()
        {
            itemStatusType = EItemStatusType.PLAYER_ARMOUR,
            isRatio = false,
            variance = 5
        };
        status_Variances.Add(status_Variance);

        status_Variance = new ItemStatusVariance()
        {
            itemStatusType = EItemStatusType.PLAYER_MOVESPEED,
            isRatio = false,
            variance = -5
        };
        status_Variances.Add(status_Variance);

        model.ItemStatusEffect = status_Variances;

        itemDatas.Add(model);

        status_Variances = new List<ItemStatusVariance>();
        model = new JsonPassiveItemModel()
        {
            ItemID = 1001,
            ItemTier = 0,
            ItemPassiveType = EPassiveItemType.SUPPORTABLE,
            ItemImage = "Select 7",
            BulletName = "",
            ItemName = "아이템 2",
            ItemContent = "이동속도 상승 아이템",
            ItemPrice = 5
        };

        status_Variance = new ItemStatusVariance()
        {
            itemStatusType = EItemStatusType.PLAYER_MOVESPEED,
            isRatio = false,
            variance = 5
        };
        status_Variances.Add(status_Variance);

        model.ItemStatusEffect = status_Variances;

        itemDatas.Add(model);

        status_Variances = new List<ItemStatusVariance>();
        model = new JsonPassiveItemModel()
        {
            ItemID = 1002,
            ItemTier = 0,
            ItemPassiveType = EPassiveItemType.SUPPORTABLE,
            ItemImage = "Select 8",
            BulletName = "",
            ItemName = "아이템 3",
            ItemContent = "플레이어 체력 상승, 몬스터 체력 상승 아이템",
            ItemPrice = 3
        };

        status_Variance = new ItemStatusVariance()
        {
            itemStatusType = EItemStatusType.PLAYER_MAXHP,
            isRatio = false,
            variance = 15
        };
        status_Variances.Add(status_Variance);

        status_Variance = new ItemStatusVariance()
        {
            itemStatusType = EItemStatusType.MONSTER_MAXHP,
            isRatio = false,
            variance = 5
        };
        status_Variances.Add(status_Variance);

        model.ItemStatusEffect = status_Variances;

        itemDatas.Add(model);

        status_Variances = new List<ItemStatusVariance>();
        model = new JsonPassiveItemModel()
        {
            ItemID = 1003,
            ItemTier = 0,
            ItemPassiveType = EPassiveItemType.SUPPORTABLE,
            ItemImage = "Select 9",
            BulletName = "",
            ItemName = "아이템 4",
            ItemContent = "플레이어 공격력 상승",
            ItemPrice = 5
        };

        status_Variance = new ItemStatusVariance()
        {
            itemStatusType = EItemStatusType.PLAYER_DAMAGE,
            isRatio = false,
            variance = 10
        };
        status_Variances.Add(status_Variance);

        model.ItemStatusEffect = status_Variances;

        itemDatas.Add(model);

        status_Variances = new List<ItemStatusVariance>();
        model = new JsonPassiveItemModel()
        {
            ItemID = 1004,
            ItemTier = 0,
            ItemPassiveType = EPassiveItemType.SUPPORTABLE,
            ItemImage = "Select 1",
            BulletName = "",
            ItemName = "아이템 5",
            ItemContent = "플레이어 근거리 공격력 상승, 플레이어 원거리 공격력 저하",
            ItemPrice = 5
        };

        status_Variance = new ItemStatusVariance()
        {
            itemStatusType = EItemStatusType.PLAYER_MELEEDAMAGE,
            isRatio = false,
            variance = 10
        };
        status_Variances.Add(status_Variance);

        status_Variance = new ItemStatusVariance()
        {
            itemStatusType = EItemStatusType.PLAYER_RANGEDAMAGE,
            isRatio = false,
            variance = -10
        };
        status_Variances.Add(status_Variance);

        model.ItemStatusEffect = status_Variances;

        itemDatas.Add(model);

        status_Variances = new List<ItemStatusVariance>();
        model = new JsonPassiveItemModel()
        {
            ItemID = 1005,
            ItemTier = 0,
            ItemPassiveType = EPassiveItemType.SUPPORTABLE,
            ItemImage = "Select 3",
            BulletName = "",
            ItemName = "아이템 6",
            ItemContent = "플레이어 원거리 공격력 상승, 플레이어 근거리 공격력 저하",
            ItemPrice = 5
        };

        status_Variance = new ItemStatusVariance()
        {
            itemStatusType = EItemStatusType.PLAYER_RANGEDAMAGE,
            isRatio = false,
            variance = 10
        };
        status_Variances.Add(status_Variance);

        status_Variance = new ItemStatusVariance()
        {
            itemStatusType = EItemStatusType.PLAYER_MELEEDAMAGE,
            isRatio = false,
            variance = -10
        };
        status_Variances.Add(status_Variance);

        model.ItemStatusEffect = status_Variances;

        itemDatas.Add(model);

        TableLoader.SaveToJson("PassiveItem", itemDatas, "TestPassive");

    }

    public void BossMonsterDataEdit()
    {
        List<JsonBossMonsterModel> jsonBossMonsterModels = new List<JsonBossMonsterModel>();

        JsonBossMonsterModel jsonBossMonsterModel = new JsonBossMonsterModel()
        {
            BossUid = 0,
            BossName = "Boss_0",
            BossThumbnail = "Enemy 4",
            DropPieceCount = 50,
            FirstPhaseLogic = EMonsterLogicType.SEQUENCE,
            FirstPhasePattern = new EBossMonsterSkill[] { EBossMonsterSkill.HEXAGONSHOOT, EBossMonsterSkill.DASH, EBossMonsterSkill.TRIPLESHOOT },
            SecondPhaseLogic = EMonsterLogicType.LOOP,
            SecondPhasePattern = new EBossMonsterSkill[] { EBossMonsterSkill.FOLLOWMOVE, EBossMonsterSkill.CIRCLESHOOT },
            ThirdPhaseLogic = EMonsterLogicType.LOOP,
            ThirdPhasePattern = new EBossMonsterSkill[] {EBossMonsterSkill.SEQUENCECIRCLESHOOT,EBossMonsterSkill.DASH}
        };

        float[] status = new float[]
        {
            100,
            3,
            10,
            5,
            5
        };

        jsonBossMonsterModel.BossStatus = status;

        jsonBossMonsterModels.Add(jsonBossMonsterModel);

        TableLoader.SaveToJson("Monster", jsonBossMonsterModels, "TestBossMonster");

    }
}
