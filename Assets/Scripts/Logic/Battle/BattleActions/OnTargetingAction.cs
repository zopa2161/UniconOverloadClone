using Core.Data.Battle;
using Core.Enums;
using Core.Interfaces;

namespace Logic.Battle.BattleActions
{
    public class OnTargetingAction : BattleAction
    {
        public OnTargetingAction(SkillExecutionContext skillContext) : base(skillContext)
        {
        }

        public override void Execute(IBattleAPI requester, IBattleContext battleContext)
        {
            PendPassiveQueue(requester, battleContext, SkillTiming.OnTargeting);
        }
    }
}