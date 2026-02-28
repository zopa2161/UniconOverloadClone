using Core.Data.Battle;

namespace Core.Interfaces
{
    public interface IActionRequester
    {
        void PushSkillSequence(SkillExecutionContext ctx);
        void PushAction(IBattleAction action);
    }
}