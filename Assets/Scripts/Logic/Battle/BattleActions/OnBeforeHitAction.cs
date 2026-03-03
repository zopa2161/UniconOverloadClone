using Core.Data.Battle;
using Core.Data.Battle.BattleLogs;
using Core.Enums;
using Core.Interfaces;

namespace Logic.Battle.BattleActions
{
    public class OnBeforeHitAction : BattleAction
    {
        public OnBeforeHitAction(SkillExecutionContext skillContext) : base(skillContext)
        {
        }

        public override void Execute(IBattleAPI requester, IBattleContext battleContext)
        {
            //Debug.Log("Execute On OnBefore Action");
            PendPassiveQueue(requester, battleContext, SkillTiming.BeforeHit);
            requester.RecordEvent(new BeforeHitLog(BattleLogType.Movement,
                _skillContext.SkillAction.EffectOverrides[0].Effect, _skillContext.caster,_skillContext.targets));
        }
    }
}