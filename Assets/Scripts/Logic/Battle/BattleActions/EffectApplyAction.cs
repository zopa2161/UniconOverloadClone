using Core.Data.Battle;
using Core.Data.Battle.BattleLogs;
using Core.Interfaces;

namespace Logic.Battle.BattleActions
{
    public class EffectApplyAction : BattleAction
    {
        public EffectApplyAction(SkillExecutionContext ctx) : base(ctx)
        {
        }

        public override void Execute(IBattleAPI requester, IBattleContext ctx)
        {
          
            foreach (var target in _ctx.targets)
            {
                //Debug.Log($"Skill Execute Target Check : {target.Faction} {target.Data.CodeName}");
                var logs = _ctx.SkillAction.ExecuteSkillAciton(_ctx.caster, target);

                foreach (var log in logs)
                {
                    requester.RecordEvent(log);
                }
               
            }

            _isFinished = true;
        }
    }
}