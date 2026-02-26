using System.Collections.Generic;
using Core.Attributes;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Enums;
using Core.Interfaces;
using UnityEngine;

namespace Core.Data.Skills
{
    /// <summary>
    ///     Skill의 정보를 담은 SO
    /// </summary>
    [CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Skill")]
    public class Skill : IdentifiedObject
    {
        [SerializeField] private SkillType _type;
        [SerializeField, SingleEnum] private SkillTiming _timing;

        [SerializeReference] [SubclassSelector]
        private List<ITriggerCondition> _condition;

        [SerializeField] private List<SkillAction> _actions;

    
        public SkillType Type => _type;
      
        public SkillTiming Timing => _timing;
        public List<SkillAction> Actions => _actions;   
        public IScope Scope => _actions[0].Scope;

        public bool IsSatisfyPassiveCondition(CharacterInstance caster, SkillExecutionContext ctx)
        {
            if (_condition == null || _condition.Count == 0)
                return true;

            for (int i = 0; i < _condition.Count; i++)
            {
                var cond = _condition[i];

                // null 조건이 끼어있으면 실패로 처리(원하면 continue로 바꿔도 됨)
                if (cond == null)
                    return false;

                if (!cond.IsSatisfiedBy(caster, ctx))
                    return false; // 하나라도 실패하면 즉시 탈락
            }

            return true;
        }
    }
}