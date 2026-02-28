using System.Collections.Generic;
using Core.Data.Character;
using Core.Data.Skills;
using Core.Enums;

namespace Core.Data.Battle.BattleLogs
{
    public class SkillDeclareLog : BattleLogEvent
    {
        public SkillInstance Skill;
        public List<CharacterInstance> Targets;

        public SkillDeclareLog(CharacterInstance caster, List<CharacterInstance> target, SkillInstance skill)
            : base(BattleLogType.SkillDeclare, caster)
        {
            Skill = skill;
            Targets = target;

            log = $"{caster.Data.CodeName} {caster.Faction} Declare {skill.Data.CodeName}";
        }

        public SkillDeclareLog(BattleLogType type, CharacterInstance actor) : base(type, actor)
        {
        }
    }
}