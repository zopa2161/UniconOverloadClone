using System;
using Core.Attributes;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Interfaces;

namespace Logic.Battle.Skills.TriggerConditions
{
    [Serializable]
    [ConditionDescription("컨텍스트의 타겟 중에 자신이 있는지 확인하여 발동 여부를 결정합니다.")]
    public class CounterTriggerCondition : ITriggerCondition
    {
        public bool IsSatisfiedBy(CharacterInstance caster, SkillExecutionContext ctx)
        {
            var result = ctx.targets.Find(c => c == caster);
            return result != null;
        }
    }
}