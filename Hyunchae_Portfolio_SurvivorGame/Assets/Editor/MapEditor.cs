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

        if (GUILayout.Button("맵 생성", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            mapCreator.GenerateMap(mapSetting.mapDatas.mapWidth,mapSetting.mapDatas.mapHeight);
        }

        if (GUILayout.Button("맵 제거", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            tilemap.ClearAllTiles();
        }

        //if (GUILayout.Button("맵 저저저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        //{
        //    //mapCreator.GenerateMap(mapSetting.mapDatas.mapWidth, mapSetting.mapDatas.mapHeight);
        //    SaveMapData();
        //}

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("맵 데이터 저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            SetTileData();

            TableLoader.SaveToJson("Map", mapSetting.mapDatas, "MapData");
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("맵 데이터 읽기", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            MapData mapData = TableLoader.LoadFromFile<MapData>("Map/MapData");

            mapSetting.mapDatas = mapData;

            mapCreator.GenerateMap(mapSetting.mapDatas.mapWidth, mapSetting.mapDatas.mapHeight);
        }

        if (GUILayout.Button("캐릭터", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            Test();
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

    private void Test()
    {
        List<CharacterModel> characters = new List<CharacterModel>();
        Status_Variance variance = new Status_Variance();

        CharacterModel characterModel = new CharacterModel
        {
            character_Name = "테스트캐릭터",
            unlockID = 1,
            unique_Ability_IDArr = new int[]{ 1, 2 },
            character_thumbnail = "Assets/Resources/Sprites/Enemy 0.png"
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
            character_Name = "테스트캐릭터2",
            unlockID = 2,
            unique_Ability_IDArr = new int[] { 3, 4 },
            character_thumbnail = "Assets/Resources/Sprites/Enemy 1.png"
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
}