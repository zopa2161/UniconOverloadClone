using System;

namespace Core.Enums
{
    [Flags]
    public enum EffectType
    {
        None = 0,
        Attack = 1 << 0,
        Heal = 1 << 1,
        Buff = 1 << 2,
        DeBuff = 1 << 3
    }
}