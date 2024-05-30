using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

//[CustomEditor(typeof(SpawnSetting))]
//public class SpawnSettingEditor : Editor
//{
//    private SpawnSetting spawnSetting;

//    private ReorderableList stageList;

//    public void OnEnable()
//    {
//        spawnSetting = target as SpawnSetting;

//        stageList = new ReorderableList(
//            serializedObject,
//            serializedObject.FindProperty("stageDatas"),
//            true,
//            true,
//            true,
//            true);

//        stageList.drawHeaderCallback = rect =>
//        {
//            EditorGUI.LabelField(rect, "Stage Data");
//        };

//        //stageList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
//        //{
//        //    //var element = stageList.serializedProperty.GetArrayElementAtIndex(index);
//        //    //EditorGUI.PropertyField(rect,index.ToString() element.FindPropertyRelative("stage"));
//        //    //EditorGUI.PropertyField(rect, element.FindPropertyRelative("stageName"));
//        //    //EditorGUI.PropertyField(rect, element.FindPropertyRelative("stageTime"));
//        //    //EditorGUI.PropertyField(rect, element.FindPropertyRelative("enemies"));
//        //    //rect.y += 2;
//        //    //EditorGUI.PropertyField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
//        //    //    element.FindPropertyRelative("stage"), GUIContent.none);
//        //    //EditorGUI.PropertyField(new Rect(rect.x + 60, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight),
//        //    //    element.FindPropertyRelative("stageName"), GUIContent.none);
//        //    //EditorGUI.PropertyField(new Rect(rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight),
//        //    //    element.FindPropertyRelative("stageTime"), GUIContent.none);
//        //    //EditorGUI.PropertyField(new Rect(rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight),
//        //    //    element.FindPropertyRelative("enemies"), GUIContent.none);
//        //};
//    }

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        //serializedObject.Update();
//        //stageList.DoLayoutList();
        
//        //serializedObject.ApplyModifiedProperties();

//        if (GUILayout.Button("WriteStageInfo", GUILayout.MinWidth(300), GUILayout.MaxWidth(600))) spawnSetting.DebugLogStageData(0);
//    }
//}
