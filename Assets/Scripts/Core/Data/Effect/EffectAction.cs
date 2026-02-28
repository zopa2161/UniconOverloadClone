using System;
using System.Collections.Generic;
using Core.Data.Battle.BattleLogs;
using Core.Data.Character;

namespace Core.Data.Effect
{
    [Serializable]
    public abstract class EffectAction : ICloneable
    {
        private ICloneable _cloneableImplementation;
        public abstract object Clone();
        public abstract float ApplyAction(CharacterInstance caster, CharacterInstance target);
    }
}