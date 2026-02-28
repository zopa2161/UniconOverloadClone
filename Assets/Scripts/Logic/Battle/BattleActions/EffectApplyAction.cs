using Core.Data.Battle;
using Core.Data.Battle.BattleLogs;
using Core.Interfaces;
using UnityEngine;

namespace Logic.Battle.BattleActions
{
    public class EffectApplyAction : BattleAction
    {
        public EffectApplyAction(SkillExecutionContext ctx) : base(ctx)
        {
            
        }

        public override void Execute(IBattleAPI requester, IBattleContext ctx)
        {        
            //Debug.Log("Execute Apply Effect Action");
            //Debug.Log($"Skill Execute Target Count : {_ctx.targets.Count}");
            foreach (var target in _ctx.targets)
            {
                
                //Debug.Log($"Skill Execute Target Check : {target.Faction} {target.Data.CodeName}");
                _ctx.SkillAction.ExecuteSkillAciton(_ctx.caster, target);
                foreach (var effectOverride in _ctx.SkillAction.EffectOverrides)
                {
                    var effect = effectOverride.Effect;
                    var log = new ApplyEffectLog(effect, _ctx.caster, _ctx.targets);
                    requester.RecordEvent(log);
                }
            }
            
            _isFinished = true;
        }
    }
}