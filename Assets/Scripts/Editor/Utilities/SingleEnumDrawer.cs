using System;
using Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor.Utilities
{
    [CustomPropertyDrawer(typeof(SingleEnumAttribute))]
    public class SingleEnumDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // 1. 현재 선택된 int 값을 진짜 Enum 타입으로 변환
            Enum currentEnum = (Enum)Enum.ToObject(fieldInfo.FieldType, property.intValue);

            // 2. 💡 핵심: 다중 선택기 대신, 단일 선택 팝업(EnumPopup)으로 강제 렌더링!
            Enum selectedEnum = EditorGUI.EnumPopup(position, label, currentEnum);

            // 3. 유저가 선택한 값을 다시 int로 변환해서 프로퍼티에 저장
            property.intValue = Convert.ToInt32(selectedEnum);

            EditorGUI.EndProperty();
        }
        
    }
}