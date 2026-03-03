using System;
using System.Collections.Generic;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Data.Targeting;
using Core.Interfaces;

namespace Logic.Battle.Skills.Tactics
{
    [Serializable]
    public class SameTargetTactic : ITactic
    {
        public List<TargetGroup> EvaluateAndFilterTargetGroups(List<TargetGroup> candidates, IBattleContext ctx,
            SkillExecutionContext skillContext)
        {
            if (candidates == null || candidates.Count == 0)
                return new List<TargetGroup>();

            var originalTarget = skillContext.targets;
            var og1 = originalTarget[0];

            var result = new List<TargetGroup>();
            result.Add(new TargetGroup(new List<CharacterInstance> { og1 }));
            return result;
        }
    }
}