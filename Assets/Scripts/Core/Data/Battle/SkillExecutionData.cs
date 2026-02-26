using System.Collections.Generic;
using Core.Data.Character;
using Core.Data.Skills;

namespace Core.Data.Battle
{
    public struct SkillExecutionData
    {
        public CharacterInstance caster;
        public SkillInstance skill;
        public List<CharacterInstance> targets;
    }
}