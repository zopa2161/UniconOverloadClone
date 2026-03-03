using System.Collections.Generic;
using Codice.Client.BaseCommands;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Data.Targeting;
using Core.Interfaces;
using UnityEngine;

namespace Core.Data.Skills
{
    public class SkillTacticData
    {
        private static readonly List<CharacterInstance> EmptyResult = new List<CharacterInstance>(0);
        public SkillInstance Skill { get; }
        private readonly TargetingScheme _targetingScheme;
        private TacticObject _tactic1;
        private TacticObject _tactic2;

        public SkillTacticData(SkillInstance skill)
        {
            Skill = skill;
            _targetingScheme = new TargetingScheme(skill.Data, _tactic1, _tactic2);
        }

        public SkillTacticData(SkillInstance skill, TacticObject tactic1, TacticObject tactic2)
        {
            Skill = skill;
            _targetingScheme = new TargetingScheme(skill.Data, tactic1, tactic2);
        }

        

        public List<CharacterInstance> GetTarget(CharacterInstance caster, IBattleContext battleContext,
            SkillExecutionContext skillContext = null)
        {
            var targetGroups = Skill.Data.Scope.GetCandidateGroups(caster, battleContext);
            if (targetGroups == null || targetGroups.Count == 0) 
                return EmptyResult;
            
            if (_tactic1 != null)
            {
                var t1 = _tactic1.TryEvaluate
                    (targetGroups, skillContext, battleContext, out var outTargets1);
                if (!t1) return EmptyResult;
                targetGroups = outTargets1;
            }

            if (_tactic2 != null)
            {
                var t2 = _tactic2.TryEvaluate
                    (targetGroups, skillContext, battleContext, out var outTargets2);
                if (!t2) return EmptyResult;
                targetGroups = outTargets2;

            }

            if (targetGroups.Count == 0) return EmptyResult;
            
            var finalGroup = targetGroups.Count > 1
                ? targetGroups[Random.Range(0, targetGroups.Count)]
                : targetGroups[0];

            return finalGroup.Targets;
        }
    }
}