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

    private ReorderableList mapList;

    public void OnEnable()
    {
        mapSetting = target as MapSetting;

        mapList = new ReorderableList(
            serializedObject,
            serializedObject.FindProperty("mapDatas"),
            true,
            true,
            true,
            true);

        mapList.drawHeaderCallback = rect =>
        {
            EditorGUI.LabelField(rect, "Map Data");
        };

        mapList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = mapList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element.FindPropertyRelative("outlineTile"));
            EditorGUI.PropertyField(rect, element.FindPropertyRelative("innerTile"));
            EditorGUI.PropertyField(rect, element.FindPropertyRelative("mapWidth"));
            EditorGUI.PropertyField(rect, element.FindPropertyRelative("mapHeight"));
            rect.y += 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("stage"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 60, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("stageName"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("stageTime"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("enemies"), GUIContent.none);
        };
    }


    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- 맵 정보 입력툴 -");
        EditorGUILayout.Space();

        //base.OnInspectorGUI();

        //selected.monsterType = (MonsterType)EditorGUILayout.EnumPopup("몬스터 종류", selected.monsterType);

        mapSetting.mapDatas.mapWidth = EditorGUILayout.IntField("맵 가로길이",mapSetting.mapDatas.mapWidth);
        mapSetting.mapDatas.mapHeight = EditorGUILayout.IntField("맵 세로길이", mapSetting.mapDatas.mapHeight);

        if (GUILayout.Button("맵 데이터 저장", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            TableLoader tl = new TableLoader();

            tl.SaveToJson("Map", mapSetting.mapDatas, "MapData");
        }

        if (GUILayout.Button("맵 데이터 읽기", GUILayout.MinWidth(300), GUILayout.MaxWidth(600)))
        {
            TableLoader tl = new TableLoader();

            object asd = tl.LoadFromFile("Map/MapData");

            Debug.Log(asd);
        }
    }
}
