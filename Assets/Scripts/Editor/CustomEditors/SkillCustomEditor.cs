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
        private SerializedProperty _consumingPointProp;

        private void OnEnable()
        {
            base.OnEnable();
            EditorContextHelper.CurrentContext = EditorContext.SkillEditor;

            _typeProp = serializedObject.FindProperty("_type");
            _timingProp = serializedObject.FindProperty("_timing");
            _conditionsProp = serializedObject.FindProperty("_condition");
            _actionsProp = serializedObject.FindProperty("_actions");
            _consumingPointProp = serializedObject.FindProperty("_consumingPoint");



            _conditionsList = new ReorderableList(serializedObject, _conditionsProp, true, true, true, true);
            _conditionsList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Trigger Conditions");
            _conditionsList.elementHeightCallback = (index) =>
            {
                var element = _conditionsProp.GetArrayElementAtIndex(index);
    
                // 기본 프로퍼티 높이 + 위아래 여백
                float height = EditorGUI.GetPropertyHeight(element, true) + 4f; 

                var conditionType = element.managedReferenceValue?.GetType();
                if (conditionType != null)
                {
                    var attr = conditionType.GetCustomAttribute<ConditionDescriptionAttribute>(false);
                    if (attr != null && !string.IsNullOrWhiteSpace(attr.Text))
                    {
                        // HelpBox가 그려진다면 그 높이만큼 전체 영역도 늘려줍니다.
                        height += EditorGUIUtility.singleLineHeight * 2.2f + 4f;
                    }
                }
                return height;
            };
            _conditionsList.drawElementCallback = (rect, index, active, focused) =>
            {
                // 리스트 기본 여백 조정
                rect.x += 10f;
                rect.width -= 15f;
                rect.y += 2f;
    
                var element = _conditionsProp.GetArrayElementAtIndex(index);
    
                // 💡 1. 현재 프로퍼티가 차지하는 '실제 높이(펼쳐짐 포함)'를 가져옵니다.
                float propertyHeight = EditorGUI.GetPropertyHeight(element, true);
                Rect propertyRect = new Rect(rect.x, rect.y, rect.width, propertyHeight);

                // 💡 2. 겹침 해결: GUIContent.none 대신 " " (공백)을 넘겨주어 SubclassSelector가 버튼을 그릴 공간을 확보하게 합니다.
                EditorGUI.PropertyField(propertyRect, element, new GUIContent(" "), true);

                // HelpBox 로직
                var conditionType = element.managedReferenceValue?.GetType();
                if (conditionType == null) return;

                var attr = conditionType.GetCustomAttribute<ConditionDescriptionAttribute>(false);
                if (attr == null || string.IsNullOrWhiteSpace(attr.Text)) return;

                // 💡 3. 하드코딩된 위치 대신, 프로퍼티가 끝나는 지점(propertyRect.yMax) 아래에 HelpBox를 그립니다.
                float helpBoxHeight = EditorGUIUtility.singleLineHeight * 2.2f;
                Rect helpRect = new Rect(rect.x, propertyRect.yMax + 4f, rect.width, helpBoxHeight);
    
                EditorGUI.HelpBox(helpRect, attr.Text, MessageType.Info);
            };
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_consumingPointProp);
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