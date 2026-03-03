using System;
using Core.Attributes;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Interfaces;

namespace Logic.Battle.Skills.TriggerConditions
{
    [Serializable]
    [ConditionDescription("컨텍스트의 스킬이 단일 대상을 목적으로 한지 여부로 판단합니다.")]
    public class SingleTargetCondition : ITriggerCondition
    {
        public bool IsSatisfiedBy(CharacterInstance caster, SkillExecutionContext ctx)
        {
            return ctx.targets.Count == 1;
        }
    }
}