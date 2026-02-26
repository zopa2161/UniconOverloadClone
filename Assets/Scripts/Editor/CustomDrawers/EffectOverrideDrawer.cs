using Core.Data.Effect;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomDrawers
{
    [CustomPropertyDrawer(typeof(EffectOverride), true)]
    public class EffectOverrideDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var effectProperty = property.FindPropertyRelative("_effect");
            var effectActionOverrideProperty = property.FindPropertyRelative("_effectActionOverride");

            var lineHeight = EditorGUIUtility.singleLineHeight;
            var spacing = EditorGUIUtility.standardVerticalSpacing;

            var effectRect = new Rect(position.x, position.y, position.width, lineHeight);
            var effectActionRect = new Rect(position.x, position.y + spacing, position.width,
                EditorGUI.GetPropertyHeight(effectActionOverrideProperty, true));

            EditorGUI.BeginChangeCheck();
            if (effectProperty == null) return;
            EditorGUI.PropertyField(effectRect, effectProperty);
            //이펙트 값 변화여부.
            var effectChanged = EditorGUI.EndChangeCheck();
            //null이 아니고, 변화 했다면 override값을 새로 만들어 줘야함.
            if (effectProperty.objectReferenceValue != null)
            {
                if (effectChanged)
                {
                    var effectObject = effectProperty.objectReferenceValue as Effect;

                    var effectActionObject = effectObject.Action;

                    if (effectActionObject == null) return;
                    //새로 만들어야함.
                    effectActionOverrideProperty.managedReferenceValue = effectActionObject.Clone() as EffectAction;
                }
            }
            else
            {
                //이펙트가 null이고, 변경이 일어난거면, 이펙트가 제거된 것이므로, 기존에 있던
                //액션 오버라이드를 제거해 줘야함.
                if (effectChanged) effectActionOverrideProperty.managedReferenceValue = null;
            }

            EditorGUI.PropertyField(effectActionRect, effectActionOverrideProperty, true);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lineHeight = EditorGUIUtility.singleLineHeight;
            var spacing = EditorGUIUtility.standardVerticalSpacing;
            var effectActionOverrideProp = property.FindPropertyRelative("_effectActionOverride");
            var effectActionHeight = EditorGUI.GetPropertyHeight(effectActionOverrideProp, true);


            return lineHeight + spacing + effectActionHeight;
        }
    }
}