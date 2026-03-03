using System;
using System.Collections.Generic;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Data.Targeting;
using Core.Interfaces;

namespace Logic.Battle.Skills.Tactics
{
    [Serializable]
    public class CounterTactic : ITactic
    {
        public List<TargetGroup> EvaluateAndFilterTargetGroups(List<TargetGroup> candidates, IBattleContext ctx,
            SkillExecutionContext skillContext = null)
        {
            if (candidates == null || candidates.Count == 0)
                return new List<TargetGroup>();
            //Debug.Log($"{skillContext.caster.Data.CodeName}");
            var caster = skillContext?.caster;
            var result = new List<TargetGroup>();
            result.Add(new TargetGroup(new List<CharacterInstance> { caster }));
            return result;
        }
    }
}