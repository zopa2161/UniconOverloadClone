using System.Collections.Generic;
using Core.Data.Character;
using Core.Enums;

namespace Core.Data.Battle.BattleLogs
{
    public class ApplyEffectLog : BattleLogEvent
    {
        public Effect.Effect Effect;
        public List<CharacterInstance> Targets;

        public ApplyEffectLog(Effect.Effect effect, CharacterInstance caster, List<CharacterInstance> targets) :
            base(BattleLogType.ApplyEffect,caster )
        {
            Effect = effect;
            Targets = targets;
            log = $"{caster.Data.CodeName} {caster.Faction} Apply {effect.CodeName} to {targets.Count} targets";
        }
        public ApplyEffectLog(BattleLogType type, CharacterInstance actor) : base(type, actor)
        {
        }
    }
}