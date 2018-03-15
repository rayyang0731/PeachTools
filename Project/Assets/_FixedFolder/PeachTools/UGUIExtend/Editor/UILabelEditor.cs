using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (UILabel))]
public class UILabelEditor : UnityEditor.UI.TextEditor {
    public override void OnInspectorGUI () {
        UILabel label = target as UILabel;

        EditorGUILayout.BeginHorizontal ();
        label.ColorKey = EditorGUILayout.TextField ("Color Key", label.ColorKey);
        if (GUILayout.Button ("Update")) {
            label.UpdateColorKey ();
        }
        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.BeginHorizontal ();
        label.LanaguageKey = EditorGUILayout.TextField ("Language Key", label.LanaguageKey);
        if (GUILayout.Button ("Update")) {
            Language.LazyLoad (Config.Get<string> ("DefaultLanguage"));
            label.UpdateLanguageKey ();
        }
        EditorGUILayout.EndHorizontal ();

        base.OnInspectorGUI ();
        serializedObject.ApplyModifiedProperties ();
    }
}