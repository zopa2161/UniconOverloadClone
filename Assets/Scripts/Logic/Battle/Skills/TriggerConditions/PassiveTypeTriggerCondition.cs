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
    [ConditionDescription("발동 원인이 된 스킬이 패시브 스킬일 경우, 이 패시브의 연쇄 발동을 허용할지 여부를 결정합니다.")]
    public class PassiveTypeTriggerCondition : ITriggerCondition
    {
        [SerializeField] private bool _reactToPassive;

        public bool IsSatisfiedBy(CharacterInstance caster, SkillExecutionContext ctx)
        {
            return ctx.Skill.Type != SkillType.Passive || _reactToPassive;
        }
    }
}