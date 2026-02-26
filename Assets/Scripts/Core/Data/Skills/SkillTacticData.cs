using System.Collections.Generic;
using Core.Data.Character;
using Core.Data.Targeting;
using Core.Interfaces;
using UnityEngine;

namespace Core.Data.Skills
{
    public class SkillTacticData
    {
        private TacticObject _tactic1;
        private TacticObject _tactic2;

        public SkillInstance Skill { get; }
        
        private TargetingScheme _targetingScheme;

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
        
        public List<CharacterInstance> GetTarget(CharacterInstance caster, IBattleContext ctx)
        {
            var result = _targetingScheme.GetTarget(caster, ctx);
            //Debug.Log($"SkillTacticData result : {result[0].Faction}");
            return result;
        }
    }
}