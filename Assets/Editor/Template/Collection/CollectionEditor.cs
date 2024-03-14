#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Collection<>))]
public class CollectionEditor : Editor
{
    private SerializedProperty listProp;
    private SerializedProperty dictionaryProp;
    private SerializedProperty nameProp;

    private void OnEnable()
    {
        listProp = serializedObject.FindProperty("list");
        dictionaryProp = serializedObject.FindProperty("dictionary");
        nameProp = serializedObject.FindProperty("collectionName");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(nameProp, true);
        EditorGUILayout.PropertyField(listProp, true);
        EditorGUILayout.PropertyField(dictionaryProp, true);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif