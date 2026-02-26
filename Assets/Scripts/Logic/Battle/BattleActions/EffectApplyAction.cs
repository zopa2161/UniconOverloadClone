using Core.Data.Battle;
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
            Debug.Log("Execute Apply Effect Action");
            Debug.Log($"Skill Execute Target Count : {_ctx.targets.Count}");
            foreach (var target in _ctx.targets)
            {
                
                Debug.Log($"Skill Execute Target Check : {target.Faction} {target.Data.CodeName}");
                _ctx.SkillAction.ExecuteSkillAciton(_ctx.caster, target);
            }
            
            _isFinished = true;
        }
    }
}