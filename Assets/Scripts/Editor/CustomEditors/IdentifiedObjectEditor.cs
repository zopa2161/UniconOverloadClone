using System.Collections.Generic;
using Core;
using Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomEditors
{
    [CustomEditor(typeof(IdentifiedObject), true)]
    public class IdentifiedObjectEditor : UnityEditor.Editor
    {
        private readonly Dictionary<string, bool> foldoutExpandedesByTitle = new();
        private SerializedProperty codeNameProperty;
        private SerializedProperty displayNameProperty;
        private SerializedProperty iconProperty;
        private SerializedProperty idProperty;


        private GUIStyle textAreaStyle;

        protected virtual void OnEnable()
        {
            GUIUtility.keyboardControl = 0;

            iconProperty = serializedObject.FindProperty("icon");
            idProperty = serializedObject.FindProperty("id");
            codeNameProperty = serializedObject.FindProperty("codeName");
            displayNameProperty = serializedObject.FindProperty("displayName");
        }

        private void StyleSetup()
        {
            if (textAreaStyle == null)
            {
                textAreaStyle = new GUIStyle(EditorStyles.textArea);
                textAreaStyle.wordWrap = true;
            }
        }

        public override void OnInspectorGUI()
        {
            StyleSetup();

            serializedObject.Update();

            if (DrawFoldoutTitle("Information"))
            {
                EditorGUILayout.BeginHorizontal("HelpBox");
                iconProperty.objectReferenceValue = EditorGUILayout.ObjectField(GUIContent.none,
                    iconProperty.objectReferenceValue,
                    typeof(Sprite), false, GUILayout.Width(65));

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUI.enabled = false;
                        EditorGUILayout.PrefixLabel("ID");
                        EditorGUILayout.PropertyField(idProperty, GUIContent.none);
                        GUI.enabled = true;
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.BeginChangeCheck();
                    var prevCodeName = codeNameProperty.stringValue;

                    EditorGUILayout.DelayedTextField(codeNameProperty);
                    if (EditorGUI.EndChangeCheck())
                    {
                        var assetPath = AssetDatabase.GetAssetPath(target);
                        var newName = $"{target.GetType().Name.ToUpper()}_{codeNameProperty.stringValue}";

                        serializedObject.ApplyModifiedProperties();

                        var message = AssetDatabase.RenameAsset(assetPath, newName);
                        if (string.IsNullOrEmpty(message)) serializedObject.Update();
                        else codeNameProperty.stringValue = prevCodeName;
                    }

                    EditorGUILayout.PropertyField(displayNameProperty);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
        }

        protected bool DrawFoldoutTitle(string text)
        {
            return CustomEditorUtility.DrawFoldoutTitle(foldoutExpandedesByTitle, text);
        }
    }
}