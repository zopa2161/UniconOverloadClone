using Core.Data.Character;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomEditors
{
    [CustomEditor(typeof(CharacterData))]
    public class CharacterCustomEditor :IdentifiedObjectEditor
    {
        SerializedProperty _active;
        SerializedProperty _passive;
        SerializedProperty _stats;
        
        private void OnEnable()
        {
            base.OnEnable();
            _active  = serializedObject.FindProperty("_activeSkillData");
            _passive = serializedObject.FindProperty("_passiveSkillData");
            _stats   = serializedObject.FindProperty("_statData");
            
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            EditorGUILayout.PropertyField(_active,  new GUIContent("Active Skills"),  includeChildren: true);
            EditorGUILayout.PropertyField(_passive, new GUIContent("Passive Skills"), includeChildren: true);
            EditorGUILayout.PropertyField(_stats,   new GUIContent("Stats"),          includeChildren: true); // 리스트 펼침 [web:532]

            serializedObject.ApplyModifiedProperties(); // Undo/Prefab override 처리 흐름 [web:532]
        }
    }
}