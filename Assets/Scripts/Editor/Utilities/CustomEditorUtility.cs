using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor.Utilities
{
    public static class CustomEditorUtility
    {
        private static GUIStyle titleStyle;

        private static GUIStyle TitleStyle
        {
            get
            {
                titleStyle ??= new GUIStyle("ShurikenModuleTitle")
                {
                    font = new GUIStyle(EditorStyles.label).font,
                    fontStyle = FontStyle.Bold,
                    fontSize = 14,
                    border = new RectOffset(15, 7, 4, 4),
                    fixedHeight = 26f,
                    contentOffset = new Vector2(20f, -2f)
                };

                return titleStyle;
            }
        }

        public static bool DrawFoldoutTitle(string title, bool isExpanded, float space = 15f)
        {
            EditorGUILayout.Space(space);

            var rect = GUILayoutUtility.GetRect(new GUIContent(title), TitleStyle);
            GUI.Box(rect, title, TitleStyle);


            var currentEvent = Event.current;

            var toggleRect = new Rect(rect.x + 4f, rect.y + 4f, 13f, 13f);


            if (currentEvent.type == EventType.Repaint)
            {
                EditorStyles.foldout.Draw(toggleRect, false, false, isExpanded, false);
            }

            else if (currentEvent.type == EventType.MouseDown && rect.Contains(currentEvent.mousePosition))
            {
                isExpanded = !isExpanded;
                currentEvent.Use();
            }

            return isExpanded;
        }


        public static bool DrawFoldoutTitle(IDictionary<string, bool> foldoutExpandedesByTitle, string title,
            float space = 15f)
        {
            if (!foldoutExpandedesByTitle.ContainsKey(title))
                foldoutExpandedesByTitle[title] = true;

            foldoutExpandedesByTitle[title] = DrawFoldoutTitle(title, foldoutExpandedesByTitle[title], space);
            return foldoutExpandedesByTitle[title];
        }

        public static void DrawUnderline(float height = 1f)
        {
            var lastRect = GUILayoutUtility.GetLastRect();
            lastRect = EditorGUI.IndentedRect(lastRect);

            lastRect.y += lastRect.height;
            lastRect.height = height;
            EditorGUI.DrawRect(lastRect, Color.gray);
        }

        // public static void DeepCopySerializeReference(SerializedProperty property)
        // {
        //     if (property.managedReferenceValue == null)
        //         return;
        //
        //     property.managedReferenceValue = (property.managedReferenceValue as ICloneable).Clone();
        // }
        //
        // public static void DeepCopySerializeReferenceArray(SerializedProperty property)
        // {
        //     for (int i = 0; i < property.arraySize; i++)
        //         DeepCopySerializeReference(property.GetArrayElementAtIndex(i));
        // }
    }
}