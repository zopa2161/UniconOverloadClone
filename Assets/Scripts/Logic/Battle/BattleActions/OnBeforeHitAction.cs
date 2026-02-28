using Core.Data.Battle;
using Core.Enums;
using Core.Interfaces;
using UnityEngine;

namespace Logic.Battle.BattleActions
{
    public class OnBeforeHitAction : BattleAction
    {
        public OnBeforeHitAction(SkillExecutionContext ctx) : base(ctx)
        {
        }

        public override void Execute(IBattleAPI requester, IBattleContext ctx)
        {
            //Debug.Log("Execute On OnBefore Action");
            PendPassiveQueue(requester, ctx, SkillTiming.BeforeHit);
        }
    }
}