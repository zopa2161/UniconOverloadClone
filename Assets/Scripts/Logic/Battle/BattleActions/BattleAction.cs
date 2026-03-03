using System.Collections.Generic;
using Core.Data.Battle;
using Core.Data.Skills;
using Core.Enums;
using Core.Interfaces;

namespace Logic.Battle.BattleActions
{
    public abstract class BattleAction : IBattleAction
    {
        protected SkillExecutionContext _skillContext;

        protected List<SkillInstance> _executedPassiveSkills;

        protected bool _isFinished;

        protected Queue<SkillExecutionData> _pendingPassivesQueue;

        public BattleAction(SkillExecutionContext skillContext)
        {
            _skillContext = skillContext;
        }

        protected BattleAction()
        {
            //throw new NotImplementedException();
        }


        public bool IsFinished => _isFinished;

        public abstract void Execute(IBattleAPI requester, IBattleContext battleContext);

        protected void PendPassiveQueue(IBattleAPI requester, IBattleContext battleContext, SkillTiming timing)
        {
            if (_skillContext != null && _skillContext.caster.IsDead)
            {
                _isFinished = true;
                return;
            }

            var isMatched = _skillContext.SkillAction.TriggerTiming.HasFlag(timing);
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


                var passiveReactors = requester.RequestPassive(timing, _skillContext);


                foreach (var data in passiveReactors)
                    //if(data.caster.스킬 사용가능해
                    //Debug.Log($"Passive React {data.skill.Data.CodeName}");
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
        }
    }
}