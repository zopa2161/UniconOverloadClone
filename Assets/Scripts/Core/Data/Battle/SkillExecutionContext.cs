using System.Collections.Generic;
using Core.Data.Character;
using Core.Data.Skills;

namespace Core.Data.Battle
{
    public class SkillExecutionContext
    {
        public CharacterInstance caster;

        public int contextDepth;
        public List<CharacterInstance> targets;


        public SkillExecutionContext(Skill skill, SkillAction skillAction, CharacterInstance caster,
            List<CharacterInstance> targets, SkillExecutionContext parentContext = null)
        {
            SkillAction = skillAction;
            this.caster = caster;
            this.targets = targets;
            ParentContext = parentContext;
            Skill = skill;
            contextDepth = ParentContext == null ? 0 : ParentContext.contextDepth + 1;
        }

        public SkillExecutionContext ParentContext { get; }
        public SkillAction SkillAction { get; private set; }
        public Skill Skill { get; }


        // (보너스) 최초로 이 사건(체인)을 일으킨 원흉을 찾는 함수
        public CharacterInstance GetOriginalAttacker()
        {
            if (ParentContext == null) return caster;
            return ParentContext.GetOriginalAttacker();
        }
    }
}