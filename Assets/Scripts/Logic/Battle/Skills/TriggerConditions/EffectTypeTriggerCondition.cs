using System;
using Core.Attributes;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Enums;
using Core.Interfaces;
using UnityEngine;

namespace Logic.Battle.Skills.TriggerConditions
{
    [Serializable]
    [ConditionDescription("컨텍스트 스킬 액션의 이펙트 타입으로 패시브 발동 여부를 결정합니다.")]
    public class EffectTypeTriggerCondition : ITriggerCondition
    {
        [SerializeField] private EffectType _type;

        public bool IsSatisfiedBy(CharacterInstance caster, SkillExecutionContext ctx)
        {
            //Debug.Log($"Effect Type Trigger Condition {ctx.SkillAction.EffectType} & {_type} = {ctx.SkillAction.EffectType & _type}");
            return (ctx.SkillAction.EffectType & _type) != 0;
        }
    }
}