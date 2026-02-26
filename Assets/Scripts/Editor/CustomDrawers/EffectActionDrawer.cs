using System.Reflection;
using Core.Attributes;
using Core.Data.Effect;
using Core.Enums;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomDrawers
{
    [CustomPropertyDrawer(typeof(EffectAction), true)]
    public class EffectActionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.managedReferenceValue == null) return;

            EditorGUI.BeginProperty(position, label, property);
            //얘는 다양한 클래스가 올 수 있으므로 타입을 알아 놔야한다.
            var targetType = property.managedReferenceValue.GetType();

            var lineHeight = EditorGUIUtility.singleLineHeight;
            var spacing = EditorGUIUtility.standardVerticalSpacing;

            var fields =
                targetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var i = 1;
            foreach (var field in fields)
            {
                //필드가 수정 가능한 형태인지 확인.
                var isSerializable = field.IsPublic || field.GetCustomAttribute<SerializeField>() != null;
                if (!isSerializable)
                    continue;
                var context = field.GetCustomAttribute<EditorContextAttribute>();
                if (context == null || context.Context == EditorContextHelper.CurrentContext)
                {
                    var childProp = property.FindPropertyRelative(field.Name);
                    var rect = new Rect(position.x, position.y + i++ * (lineHeight + 3), position.width, lineHeight);
                    EditorGUI.PropertyField(rect, childProp, true);
                }
                else
                {
                    GUI.enabled = false;
                    var childProp = property.FindPropertyRelative(field.Name);
                    var rect = new Rect(position.x, position.y + i++ * (lineHeight + 3), position.width, lineHeight);
                    EditorGUI.PropertyField(rect, childProp, true);
                    GUI.enabled = true;
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = 0f;
            if (property.managedReferenceValue == null)
                return EditorGUIUtility.singleLineHeight;

            var targetType = property.managedReferenceValue.GetType();
            var fields =
                targetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var numOfFields = fields.Length;
            height += (EditorGUIUtility.singleLineHeight + 3) * numOfFields;
            return height > 0 ? height + EditorGUIUtility.singleLineHeight : EditorGUIUtility.singleLineHeight;
        }
    }
}