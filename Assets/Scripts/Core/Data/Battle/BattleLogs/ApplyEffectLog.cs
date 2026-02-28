using System.Collections.Generic;
using Core.Data.Character;
using Core.Enums;

namespace Core.Data.Battle.BattleLogs
{
    public class ApplyEffectLog : BattleLogEvent
    {
        public Effect.Effect Effect;
        public float Effection;
        public CharacterInstance Target;

        public ApplyEffectLog(Effect.Effect effect,float effection, CharacterInstance caster, CharacterInstance target) :
            base(BattleLogType.ApplyEffect, caster)
        {
            Effection = effection;
            Effect = effect;
            Target = target;
            log = $"{caster.Data.CodeName} {caster.Faction} Apply {effect.CodeName} {effection} to {target.Data.CodeName}";
        }

        public ApplyEffectLog(BattleLogType type, CharacterInstance actor) : base(type, actor)
        {
        }
    }
}