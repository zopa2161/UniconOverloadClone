using System.Collections.Generic;
using Core.Data.Character;
using Core.Data.Skills;
using Core.Enums;

namespace Core.Data.Battle.BattleLogs
{
    public class SkillDeclareLog : BattleLogEvent
    {
        public CharacterInstance Actor;
        public SkillInstance Skill;
        public List<CharacterInstance> Targets;

        public SkillDeclareLog(SkillInstance skill, CharacterInstance caster, List<CharacterInstance> targets)
            : base(BattleLogType.SkillDeclare, caster,targets)
        {
            Actor = caster;
            Skill = skill;
            Targets = targets;

            log = $"{caster.Data.CodeName} {caster.Faction} Declare {skill.Data.CodeName}";
        }
    }
}