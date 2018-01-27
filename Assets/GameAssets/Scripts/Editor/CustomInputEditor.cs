using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(PlayerInput))]
public class CustomInputEditor : Editor {

    private ReorderableList customKeysList;
    private ReorderableList customAxisList;

    private SerializedProperty storePlayerPref;

    private void OnEnable()
    {
        storePlayerPref = serializedObject.FindProperty("storePlayerPref");


        customKeysList = new ReorderableList(serializedObject,serializedObject.FindProperty("CustomKeys"), true, true, true, true);
        customKeysList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = customKeysList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;

                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, (rect.width - 180), EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("action"), GUIContent.none);

                EditorGUI.PropertyField(
                    new Rect(rect.x + (rect.width - 180), rect.y, 70, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("defPrimary"), GUIContent.none);

                EditorGUI.PropertyField(
                    new Rect(rect.x + rect.width - 110, rect.y, 110, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("defSecondary"), GUIContent.none);
            };

        customKeysList.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Custom Keys");
        };




        customAxisList = new ReorderableList(serializedObject, serializedObject.FindProperty("CustomAxis"), true, true, true, true);

        customAxisList.elementHeight = EditorGUIUtility.singleLineHeight * 7f;
        customAxisList.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = customAxisList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 0, rect.width, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("name"), new GUIContent("Axis Name:"));
            
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 1f, rect.width, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("positive"), new GUIContent("Positive Key:"));

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2f, rect.width, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("negative"), new GUIContent("Negative Key:"));

            EditorGUI.Slider(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 3f, rect.width, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("sensitivity"), 0, 10, new GUIContent("Sensitivity:"));

            EditorGUI.Slider(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 4f, rect.width, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("gravity"), 0, 10, new GUIContent("Gravity:"));

            EditorGUI.Slider(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 5f, rect.width, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("deadzone"), 0 , 1, new GUIContent("Deadzone:"));

        };

        customAxisList.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Custom Axis");
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(storePlayerPref);

        customKeysList.DoLayoutList();
        customAxisList.DoLayoutList();
        
        serializedObject.ApplyModifiedProperties();
    }
}
