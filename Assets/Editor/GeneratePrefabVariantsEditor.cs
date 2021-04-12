using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GeneratePrefabVariants))]
public class GeneratePrefabVariantsEditor : Editor
{
    override public void OnInspectorGUI()
    {
        GeneratePrefabVariants generatePrefabVariants = (GeneratePrefabVariants)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Generate"))
        {
            generatePrefabVariants.Generate();
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Draw random point"))
        {
            generatePrefabVariants.DrawRandomPoint();
        }
        if (GUILayout.Button("Non-transparent point"))
        {
            generatePrefabVariants.DrawRandomNonTransparentPoint();
        }
        if (GUILayout.Button("Stop drawing"))
        {
            generatePrefabVariants.StopDrawing();
        }
        EditorGUILayout.EndHorizontal();
    }
}
