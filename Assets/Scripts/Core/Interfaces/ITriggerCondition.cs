using Core.Data.Battle;
using Core.Data.Character;

namespace Core.Interfaces
{
    public interface ITriggerCondition
    {
        bool IsSatisfiedBy(CharacterInstance caster, SkillExecutionContext ctx);
    }
}