using System;
using Core.Attributes;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Interfaces;
using UnityEngine;

namespace Logic.Battle.Skills.TriggerConditions
{
    [Serializable]
    [ConditionDescription("컨텍스트의 캐스터와 스킬 캐스터의 일치 여부로 패시브 발동 여부를 결정합니다.")]
    public class IsContextCasterTriggerCondition : ITriggerCondition
    {
        // true  => parameter caster == ctx.caster 여야 만족
        // false => parameter caster != ctx.caster 여야 만족
        [SerializeField] private bool _mustMatch = true;

        public bool IsSatisfiedBy(CharacterInstance caster, SkillExecutionContext ctx)
        {
            if (ctx == null) return false;

            // UnityEngine.Object 파생이면 == 가 파괴된 객체 처리까지 포함할 수 있어서 ReferenceEquals가 더 “진짜 동일 객체”에 가깝다.
            var same = ReferenceEquals(caster, ctx.caster);

            return _mustMatch ? same : !same;
        }
    }
}