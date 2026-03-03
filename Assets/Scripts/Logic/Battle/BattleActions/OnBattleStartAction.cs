using System.Collections.Generic;
using Core.Data.Battle;
using Core.Data.Battle.BattleLogs;
using Core.Enums;
using Core.Interfaces;

namespace Logic.Battle.BattleActions
{
    public class OnBattleStartAction : BattleAction
    {
        public override void Execute(IBattleAPI requester, IBattleContext battleContext)
        {
            //PendPassiveQueue(requester, ctx, SkillTiming.OnBattleStart);

            if (_pendingPassivesQueue == null)
            {
                _pendingPassivesQueue = new Queue<SkillExecutionData>();

                var passiveReactors = requester.RequestPassive(SkillTiming.OnBattleStart, _skillContext);

                foreach (var data in passiveReactors)
                    if (data.caster.GetStatValue(StatType.PP) > 0)
                        _pendingPassivesQueue.Enqueue(data);
            }

            if (_pendingPassivesQueue.Count > 0)
            {
                var pending = _pendingPassivesQueue.Dequeue();
                var declareAction = new OnDeclareAction(pending.caster, pending.targets, pending.skill, _skillContext);
                var battleManager = requester as BattleManager;
                battleManager.PushAction(declareAction);
            }

            if (_pendingPassivesQueue.Count == 0) _isFinished = true;

            requester.RecordEvent(new BattleStartLog(BattleLogType.Movement, null));
        }
    }
}