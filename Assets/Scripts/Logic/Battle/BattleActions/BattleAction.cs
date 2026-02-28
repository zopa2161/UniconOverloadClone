using System.Collections.Generic;
using Core.Data.Battle;
using Core.Data.Skills;
using Core.Enums;
using Core.Interfaces;

namespace Logic.Battle.BattleActions
{
    public abstract class BattleAction : IBattleAction
    {
        protected SkillExecutionContext _ctx;

        protected List<SkillInstance> _executedPassiveSkills;

        protected bool _isFinished;

        protected Queue<SkillExecutionData> _pendingPassivesQueue;

        public BattleAction(SkillExecutionContext ctx)
        {
            _ctx = ctx;
        }

        protected BattleAction()
        {
            //throw new NotImplementedException();
        }


        public bool IsFinished => _isFinished;

        public abstract void Execute(IBattleAPI requester, IBattleContext ctx);

        protected void PendPassiveQueue(IBattleAPI requester, IBattleContext ctx, SkillTiming timing)
        {
            var isMatched = _ctx.SkillAction.TriggerTiming.HasFlag(timing);
            //Debug.Log($"비트마스크: {_ctx.SkillAction.TriggerTiming}, 들어온값: {timing}, 매칭결과: {isMatched}");

            if (!isMatched)
            {
                //트리거를 발행하는 타이밍과 겹치지 않는다면.
                _isFinished = true;
                return;
            }

            if (_pendingPassivesQueue == null)
            {
                _pendingPassivesQueue = new Queue<SkillExecutionData>();


                var passiveReactors = requester.RequestPassive(timing, _ctx);
                var safetyCount = 0;

                foreach (var data in passiveReactors)
                {
                    safetyCount++;
                    if (safetyCount > 100) break;
                    //if(data.caster.스킬 사용가능해
                    //Debug.Log($"Passive React {data.skill.Data.CodeName}");
                    if (data.caster.GetStatValue(StatType.PP) > 0) _pendingPassivesQueue.Enqueue(data);
                }
            }

            if (_pendingPassivesQueue.Count > 0)
            {
                var pending = _pendingPassivesQueue.Dequeue();
                var declareAction = new OnDeclareAction(pending.caster, pending.targets, pending.skill);
                var battleManager = requester as BattleManager;
                battleManager.PushAction(declareAction);
            }

            if (_pendingPassivesQueue.Count == 0) _isFinished = true;
        }
    }
}