using System.Reflection;
using Core.Attributes;
using Core.Data.Effect;
using Core.Data.Skills;
using Core.Enums;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Editor.CustomEditors
{
    [CustomEditor(typeof(Skill), true)]
    public class SkillEditor : IdentifiedObjectEditor
    {
        private ReorderableList _actionsList;
        private SerializedProperty _actionsProp;

        private ReorderableList _conditionsList;
        private SerializedProperty _conditionsProp;
        private SerializedProperty _timingProp;
        private SerializedProperty _typeProp;

        private void OnEnable()
        {
            base.OnEnable();
            EditorContextHelper.CurrentContext = EditorContext.SkillEditor;

            _typeProp = serializedObject.FindProperty("_type");
            _timingProp = serializedObject.FindProperty("_timing");
            _conditionsProp = serializedObject.FindProperty("_condition");
            _actionsProp = serializedObject.FindProperty("_actions");

            _conditionsList = new ReorderableList(serializedObject, _conditionsProp, true, true, true, true);
            _conditionsList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Trigger Conditions");
            _conditionsList.elementHeightCallback = index =>
            {
                var element = _conditionsProp.GetArrayElementAtIndex(index);
                // SerializeReference는 includeChildren=true로 그릴 때 높이가 커질 수 있어서 여유를 줌
                return EditorGUI.GetPropertyHeight(element, true) + 6f + EditorGUIUtility.singleLineHeight * 2.2f;
            };
            _conditionsList.drawElementCallback = (rect, index, active, focused) =>
            {
                rect.x += 10f;
                rect.width -= 10f;
                rect.y += 2f;
                var element = _conditionsProp.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element, GUIContent.none, true);

                var conditionType = element.managedReferenceValue?.GetType();
                if (conditionType == null) return;

                var attr = conditionType.GetCustomAttribute<ConditionDescriptionAttribute>(false);
                if (attr == null || string.IsNullOrWhiteSpace(attr.Text)) return;

                var helpRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2 + 2f, rect.width,
                    EditorGUIUtility.singleLineHeight * 2.2f);
                EditorGUI.HelpBox(helpRect, attr.Text, MessageType.Info);
            };
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(_typeProp);
            if (_typeProp.enumValueIndex == (int)SkillType.Passive) EditorGUILayout.PropertyField(_timingProp);

            EditorGUILayout.Space(6);
            _conditionsList.DoLayoutList();
            EditorGUILayout.Space(6);
            EditorGUILayout.PropertyField(_actionsProp);


            EditorGUILayout.Space(8);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSkillActions()
        {
            if (_actionsProp.arraySize == 0) _actionsProp.arraySize++;

            if (!DrawFoldoutTitle("SkillActions"))
                return;

            for (var i = 0; i < _actionsProp.arraySize; i++)
            {
                var skillActionProp = _actionsProp.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginVertical("HelpBox");
                {
                    EditorGUI.indentLevel += 1;
                    var targetingTypeProp = skillActionProp.FindPropertyRelative("_targetingType");
                    EditorGUILayout.PropertyField(targetingTypeProp,
                        new GUIContent("targeting Type"));
                    if (targetingTypeProp.enumValueIndex == (int)SkillActionTargetingType.CustomScheme)
                        EditorGUILayout.PropertyField(skillActionProp.FindPropertyRelative("_targetingScheme"));

                    var effectOverrideProp = skillActionProp.FindPropertyRelative("_effectOverrides");
                    var beforeSize = effectOverrideProp.arraySize;
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(effectOverrideProp);
                    if (EditorGUI.EndChangeCheck())
                        if (effectOverrideProp.arraySize > beforeSize)
                        {
                            //effectOverride 객체의 prop lastElement
                            var formerLastElement = effectOverrideProp.GetArrayElementAtIndex(beforeSize - 1);
                            var lastElement =
                                effectOverrideProp.GetArrayElementAtIndex(effectOverrideProp.arraySize - 1);

                            //effectOverride 내부의 actionOverride prop
                            var formerActionProp = formerLastElement.FindPropertyRelative("_effectActionOverride");
                            var actionProp = lastElement.FindPropertyRelative("_effectActionOverride");
                            actionProp.managedReferenceValue =
                                ((EffectAction)formerActionProp.managedReferenceValue).Clone();
                        }

                    EditorGUI.indentLevel -= 1;
                }
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Add New SkillAction"))
            {
                var lastArraySize = _actionsProp.arraySize++;
                var newElementProperty = _actionsProp.GetArrayElementAtIndex(lastArraySize);
                var prevElementProperty = _actionsProp.GetArrayElementAtIndex(lastArraySize - 1);
            }
        }
    }
}