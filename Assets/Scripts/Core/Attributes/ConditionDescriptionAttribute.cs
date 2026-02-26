using System;

namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ConditionDescriptionAttribute : Attribute
    {
        public readonly string Text;

        public ConditionDescriptionAttribute(string text)
        {
            Text = text;
        }
    }
}