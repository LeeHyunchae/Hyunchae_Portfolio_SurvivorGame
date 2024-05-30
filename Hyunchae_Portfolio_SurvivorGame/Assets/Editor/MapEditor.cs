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

        variance.characterStatus = ECharacterStatus.MAXHP;
        variance.variance = 5;

        characterModel.variances.Add(variance);
        variance = new Status_Variance();

        variance.characterStatus = ECharacterStatus.CRITICAL_CHANCE;
        variance.variance = 3;

        characterModel.variances.Add(variance);
        variance = new Status_Variance();

        variance.characterStatus = ECharacterStatus.ATTACK_SPEED;
        variance.variance = -5;

        characterModel.variances.Add(variance);
        variance = new Status_Variance();

        variance.characterStatus = ECharacterStatus.MOVE_SPEED;
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

        variance.characterStatus = ECharacterStatus.MAXHP;
        variance.variance = -5;

        characterModel2.variances.Add(variance);
        variance = new Status_Variance();

        variance.characterStatus = ECharacterStatus.DAMAGE_MULITPLIER;
        variance.variance = 25;

        characterModel2.variances.Add(variance);
        variance = new Status_Variance();

        variance.characterStatus = ECharacterStatus.MOVE_SPEED;
        variance.variance = 30;

        characterModel2.variances.Add(variance);

        characters.Add(characterModel2);

        TableLoader.SaveToJson("Character", characters, "TestCharacter");
    }

    private void WeaponTest()
    {
        List<WeaponItemModel> itemModels = new List<WeaponItemModel>();
        WeaponItemModel itemModel = new WeaponItemModel
        {
            itemUid = 0,
            itemType = EItemType.ATTACKABLE,
            itemPrice = 3,
            itemThumbnail = "Weapon 3",
            bulletImage = "Bullet 3",
            itemName = "Gun",
            attackType = EWeaponAttackType.SHOOT,
            uniqueAbilityIDArr = new int[] { 1, 2, 3 }

        };

        WeaponStatus weaponStatus = new WeaponStatus();
        weaponStatus.damage = 3;
        weaponStatus.cooldown = 3;

        itemModel.status = weaponStatus;

        itemModels.Add(itemModel);

        WeaponItemModel itemModel2 = new WeaponItemModel
        {
            itemUid = 1,
            itemType = EItemType.ATTACKABLE,
            itemPrice = 8,
            itemThumbnail = "Weapon 1",
            bulletImage = string.Empty,
            itemName = "Spear",
            attackType = EWeaponAttackType.STING,
            uniqueAbilityIDArr = new int[] { 3, 4, 5 }

        };

        WeaponStatus weaponStatus2 = new WeaponStatus();
        weaponStatus.damage = 5;
        weaponStatus.cooldown = 5;

        itemModel2.status = weaponStatus2;

        itemModels.Add(itemModel2);

        WeaponItemModel itemModel3 = new WeaponItemModel
        {
            itemUid = 2,
            itemType = EItemType.ATTACKABLE,
            itemPrice = 8,
            itemThumbnail = "Weapon 2",
            bulletImage = string.Empty,
            itemName = "Scythe",
            attackType = EWeaponAttackType.SWING,
            uniqueAbilityIDArr = new int[] { 3, 4, 5 }

        };

        WeaponStatus weaponStatus3 = new WeaponStatus();
        weaponStatus.damage = 5;
        weaponStatus.cooldown = 5;

        itemModel2.status = weaponStatus3;

        itemModels.Add(itemModel3);

        TableLoader.SaveToJson("Weapon", itemModels, "TestWeapon");
    }

    private void MonsterTest()
    {
        List<MonsterModel> monsterModels = new List<MonsterModel>();

        MonsterModel monsterModel1 = new MonsterModel()
        {
            monsterUid = 0,
            monsterName = "followMon",
            monsterThumbnail = "Enemy 0",
            logicType = EMonsterLogicType.LOOP,
            skillType = EMonsterSkillType.NONE,
            moveType = EMonsterMoveType.FOLLOW
        };

        MonsterStatus monsterStatus = new MonsterStatus()
        {
            maxHP = 10,
            damage = 1,
            cooldown = 5,
            moveSpeed = 2,
            attackRange = 5
        };

        monsterModel1.status = monsterStatus;

        monsterModels.Add(monsterModel1);

        MonsterModel monsterModel2 = new MonsterModel()
        {
            monsterUid = 1,
            monsterName = "dashMon",
            monsterThumbnail = "Enemy 1",
            logicType = EMonsterLogicType.SEQUENCE,
            skillType = EMonsterSkillType.DASH,
            moveType = EMonsterMoveType.FOLLOW
        };

        monsterStatus = new MonsterStatus()
        {
            maxHP = 10,
            damage = 1,
            cooldown = 5,
            moveSpeed = 2,
            attackRange = 5
        };

        monsterModel2.status = monsterStatus;
        monsterModels.Add(monsterModel2);

        MonsterModel monsterModel3 = new MonsterModel()
        {
            monsterUid = 2,
            monsterName = "shootingmon",
            monsterThumbnail = "Enemy 2",
            logicType = EMonsterLogicType.LOOP,
            skillType = EMonsterSkillType.SHOOTING,
            moveType = EMonsterMoveType.FOLLOW
        };

        monsterStatus = new MonsterStatus()
        {
            maxHP = 10,
            damage = 1,
            cooldown = 5,
            moveSpeed = 2,
            attackRange = 5
        };

        monsterModel3.status = monsterStatus;
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
                stageUID = i,
                stageInfo = i + " 스테이지",
                waveGroupUID = i
            };
            stageDatas[i] = stage;
        }

        TableLoader.SaveToJson("Stage", stageDatas, "TestStage");

        WaveGroupData[] waveGroupDatas = new WaveGroupData[6];

        for(int i = 0; i <6; i++)
        {
            WaveGroupData waveGroupData = new WaveGroupData()
            {
                waveGruopUID = i,
                waveUIDArr = new int[16] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }
            };

            waveGroupDatas[i] = waveGroupData;
        }

        TableLoader.SaveToJson("Stage", waveGroupDatas, "TestWaveGrouop");


        MonsterSpawnData monsterSpawnData_0 = new MonsterSpawnData()
        {
            monsterUID = 0,
            monsterCount = 3,
            spawnStartTime = 3,
            spawnEndTime = 90,
            respawnCycleTile = 5
        };

        MonsterSpawnData monsterSpawnData_2 = new MonsterSpawnData()
        {
            monsterUID = 2,
            monsterCount = 2,
            spawnStartTime = 5,
            spawnEndTime = 90,
            respawnCycleTile = 7
        };

        MonsterGroupData[] monsterGroupDatas = new MonsterGroupData[16];

        for (int i = 0; i <16; i++)
        {
            MonsterGroupData monsterGroupData = new MonsterGroupData()
            {
                monsterGroupUID = i,
                waveTime = 90 + i,
                monsterSpawnDatas = new List<MonsterSpawnData>()
                {
                    monsterSpawnData_0,
                    monsterSpawnData_0
                }
            };

            monsterGroupDatas[i] = monsterGroupData;
        }

        TableLoader.SaveToJson("Stage", monsterGroupDatas, "TestWaves");

    }
}
