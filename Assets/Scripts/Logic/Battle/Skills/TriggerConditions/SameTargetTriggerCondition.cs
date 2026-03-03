using Core.Attributes;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Interfaces;
using UnityEngine;

namespace Logic.Battle.Skills.TriggerConditions
{
    [System.Serializable]
    [ConditionDescription("컨텍스트의 타겟중 자신이 있는지 없는지 여부로 판단합니다")]
    public class SameTargetTriggerCondition : ITriggerCondition
    {
        [SerializeField]
        private bool isSameTarget;
        public bool IsSatisfiedBy(CharacterInstance caster, SkillExecutionContext ctx)
        {
            var result = ctx.targets.Find(c => c == caster);
            return isSameTarget? result != null : result == null;
        }
    }
}