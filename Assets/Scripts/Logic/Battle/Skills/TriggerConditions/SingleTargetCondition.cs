using System;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Interfaces;

namespace Logic.Battle.Skills.TriggerConditions
{
    [Serializable]
    public class SingleTargetCondition : ITriggerCondition
    {
        public bool IsSatisfiedBy(CharacterInstance caster, SkillExecutionContext ctx)
        {
            return ctx.targets.Count == 1;
        }
    }
}