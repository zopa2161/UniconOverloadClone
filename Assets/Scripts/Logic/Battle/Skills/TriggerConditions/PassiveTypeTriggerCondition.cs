using Core.Data.Battle;
using Core.Data.Character;
using Core.Enums;
using Core.Interfaces;
using UnityEngine;

namespace Logic.Battle.Skills.TriggerConditions
{
    [System.Serializable]
    public class PassiveTypeTriggerCondition : ITriggerCondition
    {
        [SerializeField] private bool _reactToPassive;
        public bool IsSatisfiedBy(CharacterInstance caster, SkillExecutionContext ctx) =>
            ctx.Skill.Type != SkillType.Passive || _reactToPassive;
    }
}