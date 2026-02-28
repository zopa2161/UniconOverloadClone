// using Core.Data.Battle;
// using Core.Data.Character;
// using Core.Enums;
// using Core.Interfaces;
// using UnityEngine;
//
// namespace Logic.Battle.Skills.TriggerConditions
// {
//     public class SkillTypeTriggerCondition : ITriggerCondition
//     {
//         [SerializeField] private bool isActive;
//         public bool IsSatisfiedBy(CharacterInstance caster, SkillExecutionContext ctx)
//         {
//             var Skill = ctx.Skill;
//             var skillType= Skill.Type;
//             bool result = false;
//             if (skillType == SkillType.Passive)
//             {
//                 if (isActive) result = false;
//             }
//         }
//     }
// }

