using Core.Data.Targeting;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomDrawers
{
    [CustomPropertyDrawer(typeof(TargetingScheme))]
    public class TargetingSchemeDrawer : PropertyDrawer
    {
        private const float VSpace = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var h = EditorGUIUtility.singleLineHeight; // foldout line

            if (!property.isExpanded)
                return h;

            var scopeProp = property.FindPropertyRelative("_scope");
            var tacticsProp = property.FindPropertyRelative("_tactics");

            h += VSpace + EditorGUI.GetPropertyHeight(scopeProp, true); // [web:257]
            h += VSpace + EditorGUI.GetPropertyHeight(tacticsProp, true); // [web:257]

            return h;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Foldout
            var line = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(line, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                var scopeProp = property.FindPropertyRelative("_scope");
                var tacticsProp = property.FindPropertyRelative("_tactics");

                var y = line.yMax + VSpace;

                // Scope
                var scopeH = EditorGUI.GetPropertyHeight(scopeProp, true); // [web:257]
                var scopeRect = new Rect(position.x, y, position.width, scopeH);
                EditorGUI.PropertyField(scopeRect, scopeProp,
                    true); // SubclassSelector가 여기서 그려짐 [web:259]
                y += scopeH + VSpace;

                // Tactics list
                var tacticsH = EditorGUI.GetPropertyHeight(tacticsProp, true); // [web:257]
                var tacticsRect = new Rect(position.x, y, position.width, tacticsH);
                EditorGUI.PropertyField(tacticsRect, tacticsProp, true); // [web:259]
                y += tacticsH + VSpace;

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }
    }
}