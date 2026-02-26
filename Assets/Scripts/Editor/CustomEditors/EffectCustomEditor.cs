using Core.Attributes;
using Core.Data.Effect;
using Core.Enums;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomEditors
{
    [CustomEditor(typeof(Effect))]
    public class EffectCustomEditor : IdentifiedObjectEditor
    {
        SerializedProperty _typeProp;
        SerializedProperty _actionProp;

        private void OnEnable()
        {
            base.OnEnable();

            EditorContextHelper.CurrentContext = EditorContext.EffectEditor;
            _typeProp = serializedObject.FindProperty("_type");
            _actionProp = serializedObject.FindProperty("_action");
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            EditorGUILayout.PropertyField(_typeProp);

            EditorGUILayout.Space(6);

            DrawEffectAction();

            // 선택 사항: Action이 null이면 안내
            if (_actionProp.managedReferenceValue == null) // managed ref 접근 [web:159]
            {
                EditorGUILayout.HelpBox("Effect Action이 비어있습니다. SubclassSelector로 구현체를 선택하세요.", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawEffectAction()
        {
            if (!DrawFoldoutTitle("Action"))
                return;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.PropertyField(_actionProp);
            }
        
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }
    }
}