using Core.Data.Battle;
using Core.Data.Battle.BattleLogs;
using Core.Enums;
using Core.Interfaces;

namespace Logic.Battle.BattleActions
{
    public class OnAfterHitAction : BattleAction
    {
        public OnAfterHitAction(SkillExecutionContext skillContext) : base(skillContext)
        {
        }

        public override void Execute(IBattleAPI requester, IBattleContext battleContext)
        {
            PendPassiveQueue(requester, battleContext, SkillTiming.AfterHit);
            requester.RecordEvent(new AfterHitLog(BattleLogType.Movement, _skillContext.caster));
        }
    }
}