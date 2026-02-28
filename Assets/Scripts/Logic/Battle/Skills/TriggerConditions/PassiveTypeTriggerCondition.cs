using System;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Enums;
using Core.Interfaces;
using UnityEngine;

namespace Logic.Battle.Skills.TriggerConditions
{
    [Serializable]
    public class PassiveTypeTriggerCondition : ITriggerCondition
    {
        [SerializeField] private bool _reactToPassive;

        public bool IsSatisfiedBy(CharacterInstance caster, SkillExecutionContext ctx)
        {
            return ctx.Skill.Type != SkillType.Passive || _reactToPassive;
        }
    }
}