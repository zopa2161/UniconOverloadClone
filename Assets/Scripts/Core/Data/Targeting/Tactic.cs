using System.Collections.Generic;
using Core.Data.Battle;
using Core.Interfaces;
using UnityEngine;

namespace Core.Data.Targeting
{
    public class TacticObject : ScriptableObject
    {
        [SerializeReference] private ITactic _tactic;
        [SerializeReference] private ITriggerCondition _condition;

        public ITactic Tactic => _tactic;

        public bool TryEvaluate(List<TargetGroup> candidates, SkillExecutionContext skillContext,IBattleContext battleContext, out List<TargetGroup> outTargets)
        {
            outTargets = null;
            // 상태 검사.
            // 1. 조건문이 있고 불만족하면 false 리턴
            if (_condition != null && !_condition.IsSatisfiedBy(skillContext.caster, skillContext)) return false;

            
            //조건문이 없으니까 tactic을 따져 봐야함.
            
            // 입력된 후보가 하나인 경우 out에 하나만 담아서 리턴하자
            if (candidates.Count == 1)
            {
                outTargets.Add(candidates[0]);
                return true;
            }

            if (_tactic != null)
            {
                outTargets = _tactic.EvaluateAndFilterTargetGroups(candidates, battleContext, skillContext);
            }

            return true;

        }
    }
}