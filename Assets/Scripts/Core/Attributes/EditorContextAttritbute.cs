using System;
using Core.Enums;
using UnityEngine;

namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EditorContextAttribute : PropertyAttribute
    {
        public EditorContextAttribute(EditorContext context)
        {
            Context = context;
        }

        public EditorContext Context { get; private set; }
    }

    public static class EditorContextHelper
    {
        //스태틱으로 정의되어 있고, 에디터를 옮겨다닐 때마다 변화함.
        public static EditorContext CurrentContext = EditorContext.Both;
    }
}