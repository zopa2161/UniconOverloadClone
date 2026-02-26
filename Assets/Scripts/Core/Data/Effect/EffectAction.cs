using System;
using Core.Data.Character;
using Core.Interfaces;

namespace Core.Data.Effect
{
    [Serializable]
    public abstract class EffectAction : ICloneable
    {
        private ICloneable _cloneableImplementation;
        public abstract object Clone();
        public abstract void ApplyAction(CharacterInstance caster, CharacterInstance target);
    }
}