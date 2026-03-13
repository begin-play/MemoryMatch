/*
 * Copyright (c) 2026 Sagar Kumar
 * All Rights Reserved.
 */
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    private SerializedProperty isGridSizeValidProp;

    private void OnEnable()
    {
        isGridSizeValidProp = serializedObject.FindProperty("isGridSizeValid");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        if (!isGridSizeValidProp.boolValue)
        {
            EditorGUILayout.HelpBox("Grid size must be even and between 4 and 16. And a multiple of 2 :)", MessageType.Error);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
