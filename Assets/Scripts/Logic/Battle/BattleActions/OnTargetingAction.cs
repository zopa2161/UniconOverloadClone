using System.Collections.Generic;
using Core.Data.Battle;
using Core.Data.Skills;
using Core.Enums;
using Core.Interfaces;
using PlasticGui.WorkspaceWindow.BranchExplorer;
using UnityEngine;

namespace Logic.Battle.BattleActions
{
    public class OnTargetingAction : BattleAction
    {
        public OnTargetingAction(SkillExecutionContext ctx) : base(ctx)
        {
        }

        public override void Execute(IBattleAPI requester, IBattleContext ctx)
        {
            //Debug.Log("Execute On Targeting Action");
            PendPassiveQueue(requester, ctx, SkillTiming.OnTargeting);
            //_isFinished = true;
        }
    }
}