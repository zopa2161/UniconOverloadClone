using Core.Data.Battle;
using Core.Enums;
using Core.Interfaces;

namespace Logic.Battle.BattleActions
{
    public class EffectApplyAction : BattleAction
    {
        public EffectApplyAction(SkillExecutionContext skillContext) : base(skillContext)
        {
        }

        public override void Execute(IBattleAPI requester, IBattleContext battleContext)
        {
            if (_skillContext.caster.IsDead)
            {
                _isFinished = true;
                return;
            }

            //0. 스킬 타입을 확인해서 caster의 ap,pp를 소모 시킨다.

            if (!_skillContext.HasConsumedPoint)
            {
                //Debug.Log("consuming at preBattle");
                if (_skillContext.Skill.Type == SkillType.Active)
                    _skillContext.caster.StatSystem.ConsumeSkillPoint(StatType.AP, _skillContext.Skill.ConsumingPoint);
                else if (_skillContext.Skill.Type == SkillType.Passive) _skillContext.caster.StatSystem.ConsumeSkillPoint(StatType.PP, _skillContext.Skill.ConsumingPoint );
                _skillContext.HasConsumedPoint = true;
            }

            var logs = _skillContext.SkillAction.ExecuteSkillAciton(_skillContext.caster, _skillContext.targets ,_skillContext);
            logs[0].LateConsumeInjection();
            foreach (var log in logs)
            {
                log.LateSkillInjection(_skillContext.Skill);
                requester.RecordEvent(log);
            }

            _isFinished = true;
        }
    }
}