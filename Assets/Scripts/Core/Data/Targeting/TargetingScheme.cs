using System;
using System.Collections.Generic;
using Core.Data.Character;
using Core.Data.Skills;
using Core.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Data.Targeting
{
    [Serializable]
    public class TargetingScheme : ICloneable
    {
        [SerializeReference] [SubclassSelector]
        private IScope _scope;

        [SerializeReference] [SubclassSelector]
        private List<ITactic> _tactics = new();

        public TargetingScheme(Skill skill, TacticObject tactic1, TacticObject tactic2)
        {
            _scope = skill.Scope;
            _tactics.Add(tactic1? tactic1.Tactic : null);
            _tactics.Add(tactic2 ? tactic2.Tactic : null);
        }

        public TargetingScheme(TargetingScheme original)
        {
            _scope = original._scope.Clone() as IScope;
            foreach (var tactic in original._tactics)
            {
                _tactics.Add(tactic);
            }
        }


        public IScope Scope => _scope;

        public List<CharacterInstance> GetTarget(CharacterInstance caster, IBattleContext ctx)
        {
            var candidates = _scope.GetCandidateGroups(caster, ctx);
            
            foreach (var tactic in _tactics)
            {
                //조기 탈출
                if (candidates.Count <= 1) break;
                if (tactic == null) continue;
                candidates = tactic.EvaluateAndFilterTargetGroups(candidates, ctx);
            }

            if (candidates.Count == 0) return new List<CharacterInstance>();

            var finalGroup = candidates.Count > 1
                ? candidates[Random.Range(0, candidates.Count)]
                : candidates[0];
            //Debug.Log($"finalGroup  : {finalGroup.Targets[0].Faction}");
            //Debug.Log($"Targeting Scheme Candidates Count : {candidates[0].Targets[0].Faction}");
            return finalGroup.Targets;
        }

        public object Clone()
        {
            var result = new TargetingScheme(this);
            return result;
        }
    }
}