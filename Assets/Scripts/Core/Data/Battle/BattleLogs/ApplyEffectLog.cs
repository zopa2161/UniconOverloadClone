using System.Collections.Generic;
using Core.Data.Character;
using Core.Data.Skills;
using Core.Enums;

namespace Core.Data.Battle.BattleLogs
{
    public class ApplyEffectLog : BattleLogEvent
    {
        public Effect.Effect Effect;
        public List<float> Effections; 
        public bool isConsume;
        public Skill Skill;
     


        public ApplyEffectLog(Skill skill, Effect.Effect effect, List<float> effections, CharacterInstance caster,
            List<CharacterInstance> targets) :
            base(BattleLogType.ApplyEffect, caster,targets)
        {
            Effections = effections;
            Skill = skill;
            Effect = effect;
            Targets = targets;
            log = $"{caster.Data.CodeName} {caster.Faction} Apply {effect.CodeName} ";
        }

        public ApplyEffectLog(Effect.Effect effect, List<float> effections, CharacterInstance caster,
            List<CharacterInstance> targets) : base(BattleLogType.ApplyEffect, caster, targets)
        {
            Skill = null;
            Effections = effections;
            Effect = effect;
            Targets = targets;
        
        }
        

        public void LateConsumeInjection()
        {
            isConsume = true;
        }
        public void LateSkillInjection(Skill skill)
        {
            Skill = skill;
        }
    }
}