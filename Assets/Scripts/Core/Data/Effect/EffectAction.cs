using System;
using Core.Data.Battle;
using Core.Data.Character;

namespace Core.Data.Effect
{
    [Serializable]
    public abstract class EffectAction : ICloneable
    {
        private ICloneable _cloneableImplementation;
        public abstract object Clone();
        public abstract float ApplyAction(CharacterInstance caster, CharacterInstance target, SkillExecutionContext skillContext);
    }
}