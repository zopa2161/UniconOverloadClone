using Core.Data.Battle;
using Core.Data.Battle.BattleLogs;
using Core.Enums;
using Core.Interfaces;

namespace Logic.Battle.BattleActions
{
    public class OnHitAction : BattleAction
    {
        public OnHitAction(SkillExecutionContext skillContext) : base(skillContext)
        {
        }

        public override void Execute(IBattleAPI requester, IBattleContext battleContext)
        {
            //Debug.Log("Execute On OnBefore Action");
            PendPassiveQueue(requester, battleContext, SkillTiming.OnHit);
            requester.RecordEvent(new BeforeHitLog(BattleLogType.Movement,
                _skillContext.SkillAction.EffectOverrides[0].Effect, _skillContext.caster,_skillContext.targets));
        }
    }
}